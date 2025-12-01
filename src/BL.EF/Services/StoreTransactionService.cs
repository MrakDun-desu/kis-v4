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

    public StoreTransactionCreateResponse Create(
            StoreTransactionCreateRequest req,
            int userId) {
        var reqTime = _timeProvider.GetUtcNow();
        var user = _userService.GetOrCreate(userId);

        _dbContext.Database.BeginTransaction();
        var entity = CreateInternal(req, userId, reqTime, _dbContext);

        _dbContext.SaveChanges();
        _dbContext.Database.CommitTransaction();

        // TODO: Implement returning correctly
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a StoreTransaction entity with the specified parameters. Must always be wrapped
    /// inside of a database transaction to keep the database state consistent.
    /// </summary>
    internal static StoreTransaction CreateInternal(
            StoreTransactionCreateRequest req,
            int userId,
            DateTimeOffset reqTime,
            KisDbContext dbContext,
            SaleTransaction? saleTransaction = null
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

        UpdateItemAmounts(req, transactionItemsCount, dbContext);

        dbContext.StoreTransactions.Add(entity);

        return entity;
    }

    private static void UpdateItemAmounts(
            StoreTransactionCreateRequest req,
            int itemsCount,
            KisDbContext dbContext
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

        // update all the store item amounts in one database call
        dbContext.StoreItemAmounts
            .Join(
                    storeItemAmountChanges,
                    sia => new { sia.StoreId, sia.StoreItemId },
                    c => new { c.StoreId, c.StoreItemId },
                    (sia, c) => new { StoreItemAmount = sia, Change = c.ItemAmount }
                    )
            .ExecuteUpdate(
                    props => props.SetProperty(
                        x => x.StoreItemAmount.Amount,
                        x => x.StoreItemAmount.Amount + x.Change
                        )
                    );

        // search for all the necessary compositions to update the composite amounts
        var changedStoreItemIds = req.StoreTransactionItems.Select(sti => sti.StoreItemId);
        var compositeIds = dbContext.Compositions
            .Where(c => changedStoreItemIds.Contains(c.StoreItemId))
            .Select(c => c.CompositeId)
            .Distinct()
            .ToArray();

        var compositions = dbContext.Compositions
            .Where(c => compositeIds.Contains(c.CompositeId))
            .GroupBy(c => c.CompositeId)
            .ToArray();
        var relevantStoreItemIds = compositions.SelectMany(g => g.Select(c => c.StoreItemId)).Distinct();

        List<int> storesToUpdate = [req.StoreId];
        if (req.SourceStoreId is { } sourceStoreId) {
            storesToUpdate.Add(sourceStoreId);
        }

        // get store item amounts for all the composites
        var storeItemAmounts = dbContext.StoreItemAmounts
            .Where(sia => storesToUpdate.Contains(sia.StoreId))
            .Where(sia => relevantStoreItemIds.Contains(sia.StoreItemId))
            .GroupBy(sia => sia.StoreId)
            .Select(g => new {
                StoreId = g.Key,
                ItemAmounts = g.Select(sia => new { sia.StoreItemId, sia.Amount })
            })
            .ToArray();

        var newCompositeAmounts = new List<(int CompositeId, int StoreId, decimal NewAmount)>
            (compositions.Length * storesToUpdate.Count);

        // compute the new composite amounts
        foreach (var composition in compositions) {
            foreach (var storeId in storesToUpdate) {
                var newAmount = composition
                    .Join(
                            storeItemAmounts.First(sia => sia.StoreId == storeId).ItemAmounts,
                            c => c.StoreItemId,
                            sia => sia.StoreItemId,
                            (c, sia) => new { NeededAmount = c.Amount, AvailableAmount = sia.Amount }
                            )
                    .Select(x => x.AvailableAmount / x.NeededAmount)
                    .Min();
                newCompositeAmounts.Add((composition.Key, storeId, newAmount));
            }
        }

        // update all composite amounts in one database call
        dbContext.CompositeAmounts
            .Join(
                    newCompositeAmounts,
                    ca => new { ca.StoreId, ca.CompositeId },
                    nc => new { nc.StoreId, nc.CompositeId },
                    (ca, nc) => new { Entity = ca, nc.NewAmount }
                    )
            .ExecuteUpdate(
                    props => props.SetProperty(
                        x => x.Entity.Amount,
                        x => x.NewAmount
                    ));
    }
}


