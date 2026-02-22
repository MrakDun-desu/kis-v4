using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class StoreTransactionService(
        KisDbContext dbContext,
        TimeProvider timeProvider,
        UserService userService
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly UserService _userService = userService;

    public async Task<StoreTransactionCreateResponse> CreateAsync(
            StoreTransactionCreateRequest req,
            int userId,
            CancellationToken token = default
            ) {
        var reqTime = _timeProvider.GetUtcNow();
        var user = await _userService.GetOrCreateAsync(userId, token);

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try {
            var entity = await CreateInternalAsync(req, userId, reqTime, _dbContext, token: token);
            await transaction.CommitAsync();

            return new StoreTransactionCreateResponse {
                Id = entity.Id,
                Note = entity.Note,
                Reason = entity.Reason,
                StartedAt = entity.StartedAt,
                StoreTransactionItems = entity.StoreTransactionItems.Select(sti => sti.ToModel()).ToArray(),
                CancelledAt = entity.CancelledAt,
                CancelledBy = entity.CancelledBy.ToModel(),
                SaleTransactionId = entity.SaleTransactionId,
                StartedBy = user
            };
        } catch {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Creates a StoreTransaction entity with the specified parameters. Must always be wrapped
    /// inside of a database transaction to keep the database state consistent.
    /// </summary>
    internal static async Task<StoreTransaction> CreateInternalAsync(
            StoreTransactionCreateRequest req,
            int userId,
            DateTimeOffset reqTime,
            KisDbContext dbContext,
            SaleTransaction? saleTransaction = null,
            CancellationToken token = default
            ) {
        var transactionItemsCount = req.StoreTransactionItems.Length;
        if (req.SourceStoreId.HasValue) {
            transactionItemsCount *= 2;
        }
        var entity = new StoreTransaction {
            Note = req.Note,
            Reason = req.Reason,
            SaleTransaction = saleTransaction,
            StartedAt = reqTime,
            StartedById = userId,
            StoreTransactionItems = req.StoreTransactionItems.Aggregate(
                    new List<StoreTransactionItem>(transactionItemsCount), (output, sti) => {
                        output.Add(new StoreTransactionItem {
                            StoreId = req.StoreId,
                            Cost = sti.Cost,
                            ItemAmount = sti.ItemAmount,
                            StoreItemId = sti.StoreItemId
                        });

                        if (req.SourceStoreId is { } sourceStoreId) {
                            output.Add(new StoreTransactionItem {
                                StoreId = sourceStoreId,
                                Cost = sti.Cost,
                                ItemAmount = -sti.ItemAmount,
                                StoreItemId = sti.StoreItemId
                            });
                        }
                        return output;
                    })
        };

        dbContext.StoreTransactions.Add(entity);
        await dbContext.SaveChangesAsync(token);

        await UpdateItemAmountsAsync(req, transactionItemsCount, dbContext, token);

        return entity;
    }

    private static async Task UpdateItemAmountsAsync(
            StoreTransactionCreateRequest req,
            int itemsCount,
            KisDbContext dbContext,
            CancellationToken token
            ) {
        // make a list of store item amount changes to update the store item amounts in the
        // database. There should be no need to group by store item ID since they should already be
        // distinct thanks to validation
        var storeItemAmountChanges = req.StoreTransactionItems
            .Aggregate(new List<(int StoreId, int StoreItemId, decimal ItemAmount)>(itemsCount),
                    (output, sti) => {
                        output.Add((req.StoreId, sti.StoreItemId, sti.ItemAmount));

                        // if there is a source store (this is a movement operation), also remove the items from the
                        // source store
                        if (req.SourceStoreId is { } sourceStoreId) {
                            output.Add((sourceStoreId, sti.StoreItemId, -sti.ItemAmount));
                        }
                        return output;
                    });

        List<int> storesToUpdate = [req.StoreId];
        if (req.SourceStoreId is { } sourceStoreId) {
            storesToUpdate.Add(sourceStoreId);
        }

        var storeItemsToUpdate = req.StoreTransactionItems.Select(sti => sti.StoreItemId);

        var existingStoreItemAmounts = await dbContext.StoreItemAmounts
            .Where(sia => storesToUpdate.Contains(sia.StoreId))
            .Where(sia => storeItemsToUpdate.Contains(sia.StoreItemId))
            .ToArrayAsync(token);

        var missingStoreItemAmounts = storeItemAmountChanges
            .Where(x => !existingStoreItemAmounts
                    .Any(sia => sia.StoreItemId == x.StoreItemId && sia.StoreId == x.StoreId)
                    )
            .ToArray();

        // add zero amount to every store item amount that hasn't been found so far
        foreach (var missingAmount in missingStoreItemAmounts) {
            dbContext.StoreItemAmounts.Add(
                    new StoreItemAmount {
                        StoreItemId = missingAmount.StoreItemId,
                        StoreId = missingAmount.StoreId,
                        Amount = 0
                    }
                );
        }

        await dbContext.SaveChangesAsync(token);

        // update all the store item amounts in one database call
        await dbContext.StoreItemAmounts
            .Join(
                    storeItemAmountChanges,
                    sia => new { sia.StoreId, sia.StoreItemId },
                    c => new { c.StoreId, c.StoreItemId },
                    (sia, c) => new { StoreItemAmount = sia, Change = c.ItemAmount }
                    )
            .ExecuteUpdateAsync(
                    props => props.SetProperty(
                        x => x.StoreItemAmount.Amount,
                        x => x.StoreItemAmount.Amount + x.Change
                        ),
                    token
                    );

        // search for all the necessary compositions to update the composite amounts
        var changedStoreItemIds = req.StoreTransactionItems.Select(sti => sti.StoreItemId);
        var compositeIds = await dbContext.Compositions
            .Where(c => changedStoreItemIds.Contains(c.StoreItemId))
            .Select(c => c.CompositeId)
            .Distinct()
            .ToArrayAsync(token);

        var compositions = await dbContext.Compositions
            .Where(c => compositeIds.Contains(c.CompositeId))
            .GroupBy(c => c.CompositeId)
            .ToArrayAsync(token);
        var relevantStoreItemIds = compositions.SelectMany(g => g.Select(c => c.StoreItemId)).Distinct();

        // get store item amounts for all the composites
        var newStoreItemAmounts = await dbContext.StoreItemAmounts
            .Where(sia => storesToUpdate.Contains(sia.StoreId))
            .Where(sia => relevantStoreItemIds.Contains(sia.StoreItemId))
            .GroupBy(sia => sia.StoreId)
            .Select(g => new {
                StoreId = g.Key,
                ItemAmounts = g.Select(sia => new { sia.StoreItemId, sia.Amount })
            })
            .ToArrayAsync(token);

        var newCompositeAmounts = new List<(int CompositeId, int StoreId, decimal NewAmount)>
            (compositions.Length * storesToUpdate.Count);

        // compute the new composite amounts
        foreach (var composition in compositions) {
            foreach (var storeId in storesToUpdate) {
                var newAmount = composition
                    .Join(
                            newStoreItemAmounts.First(sia => sia.StoreId == storeId).ItemAmounts,
                            c => c.StoreItemId,
                            sia => sia.StoreItemId,
                            (c, sia) => new { NeededAmount = c.Amount, AvailableAmount = sia.Amount }
                            )
                    .Select(x => x.AvailableAmount / x.NeededAmount)
                    .Min();
                newCompositeAmounts.Add((composition.Key, storeId, newAmount));
            }
        }

        var existingCompositeAmounts = await dbContext.CompositeAmounts
            .Where(ca => storesToUpdate.Contains(ca.StoreId))
            .Where(ca => compositeIds.Contains(ca.CompositeId))
            .ToArrayAsync(token);

        var missingCompositeAmounts = newCompositeAmounts
            .Where(ca => !existingCompositeAmounts
                    .Any(eca => eca.StoreId == ca.StoreId && eca.CompositeId == ca.CompositeId))
            .ToArray();

        // adding missing composite amounts
        foreach (var missingAmount in missingCompositeAmounts) {
            dbContext.CompositeAmounts.Add(new CompositeAmount {
                CompositeId = missingAmount.CompositeId,
                StoreId = missingAmount.StoreId,
                Amount = 0
            });
        }

        await dbContext.SaveChangesAsync(token);

        // update all composite amounts in one database call
        await dbContext.CompositeAmounts
            .Join(
                    newCompositeAmounts,
                    ca => new { ca.StoreId, ca.CompositeId },
                    nc => new { nc.StoreId, nc.CompositeId },
                    (ca, nc) => new { Entity = ca, nc.NewAmount }
                    )
            .ExecuteUpdateAsync(
                    props => props.SetProperty(
                        x => x.Entity.Amount,
                        x => x.NewAmount
                    ),
                    token
                );
    }
}


