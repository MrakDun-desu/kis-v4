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
        var storeTransactionItems = req.StoreTransactionItems.Aggregate(
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
            }
        );

        var entity = new StoreTransaction {
            Note = req.Note,
            Reason = req.Reason,
            SaleTransaction = saleTransaction,
            StartedAt = reqTime,
            StartedById = userId,
            StoreTransactionItems = storeTransactionItems
        };

        dbContext.StoreTransactions.Add(entity);
        await dbContext.SaveChangesAsync(token);

        await UpdateItemAmountsAsync(storeTransactionItems, entity.Id, dbContext, token);

        return entity;
    }

    private static async Task UpdateItemAmountsAsync(
            List<StoreTransactionItem> newTransactionItems,
            int transactionId,
            KisDbContext dbContext,
            CancellationToken token = default
            ) {

        var storesToUpdate = newTransactionItems
            .Select(sti => sti.StoreId)
            .Distinct()
            .ToArray();

        var storeItemsToUpdate = newTransactionItems
            .Select(sti => sti.StoreItemId)
            .Distinct()
            .ToArray();

        var existingStoreItemAmounts = await dbContext.StoreItemAmounts
            .Where(sia => storesToUpdate.Contains(sia.StoreId))
            .Where(sia => storeItemsToUpdate.Contains(sia.StoreItemId))
            .ToArrayAsync(token);

        var missingStoreItemAmounts = newTransactionItems
            .Where(x => !existingStoreItemAmounts
                    .Any(sia => sia.StoreItemId == x.StoreItemId && sia.StoreId == x.StoreId)
                    )
            .ToArray();

        // add zero amount to every store item amount that hasn't been found so far
        if (missingStoreItemAmounts.Length > 0) {
            dbContext.StoreItemAmounts
                .AddRange(missingStoreItemAmounts
                        .Select(a => new StoreItemAmount {
                            StoreItemId = a.StoreItemId,
                            StoreId = a.StoreId,
                            Amount = 0
                        })
                );

            await dbContext.SaveChangesAsync(token);
        }

        // update all the store item amounts in one database call
        await dbContext.StoreItemAmounts
            .Where(sia => dbContext.StoreTransactionItems
                    .Any(c => c.StoreId == sia.StoreId
                        && c.StoreItemId == sia.StoreItemId
                        && c.StoreTransactionId == transactionId)
                    )
            .ExecuteUpdateAsync(
                props => props.SetProperty(
                    x => x.Amount,
                    x => x.Amount + dbContext.StoreTransactionItems
                    .Where(sti => sti.StoreId == x.StoreId
                        && sti.StoreItemId == x.StoreItemId
                        && sti.StoreTransactionId == transactionId
                    )
                    .Sum(sti => sti.ItemAmount)
                ), token
            );

        // update all composite amounts in one database call
        await dbContext.CompositeAmounts
            .Where(ca => dbContext.Compositions
                    .Any(c => c.CompositeId == ca.CompositeId
                        && dbContext.StoreTransactionItems
                        .Any(sti => sti.StoreItemId == c.StoreItemId
                            && sti.StoreId == ca.StoreId
                            && sti.StoreTransactionId == transactionId)
                        )
                    )
            .ExecuteUpdateAsync(
                    props => props.SetProperty(
                        x => x.Amount,
                        x => dbContext.Compositions
                            .Where(c => c.CompositeId == x.CompositeId)
                            .Select(c => (int)(dbContext.StoreItemAmounts
                                .Where(sia => sia.StoreItemId == c.StoreItemId
                                    && sia.StoreId == x.StoreId)
                                .Sum(sia => sia.Amount) / c.Amount)
                                )
                            .Min()
                        )
                    );
    }
}


