using KisV4.Common.DependencyInjection;
using KisV4.Common.Enums;
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
        var entity = CreateInternal(req, userId, reqTime, null);

        _dbContext.SaveChanges();
        _dbContext.Database.CommitTransaction();

        // TODO: Implement returning correctly
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a StoreTransaction entity with the specified parameters. Must always be wrapped
    /// inside of a database transaction to keep the database state consistent.
    /// </summary>
    internal StoreTransaction CreateInternal(
            StoreTransactionCreateRequest req,
            int userId,
            DateTimeOffset reqTime,
            SaleTransaction? saleTransaction = null
            ) {
        // Creating store transaction itself (simple)
        var entity = new StoreTransaction {
            Note = req.Note,
            Reason = req.Reason,
            SaleTransaction = saleTransaction,
            StartedAt = reqTime,
            StartedById = userId,
            StoreTransactionItems = req.StoreTransactionItems.SelectMany(sti => {
                List<StoreTransactionItem> output = [
                    new StoreTransactionItem {
                            StoreId = req.StoreId,
                            Cost = sti.Cost,
                            ItemAmount = sti.ItemAmount,
                            StoreItemId = sti.StoreItemId
                        }
                ];

                if (req.Reason is StoreTransactionReason.ChangingStores) {
                    output.Add(new StoreTransactionItem {
                        StoreId = req.SourceStoreId!.Value,
                        Cost = sti.Cost,
                        ItemAmount = -sti.ItemAmount,
                        StoreItemId = sti.StoreItemId
                    });
                }
                return output;
            }).ToArray()
        };

        UpdateItemAmounts(req);

        _dbContext.StoreTransactions.Add(entity);

        return entity;
    }

    private void UpdateItemAmounts(StoreTransactionCreateRequest req) {
        // make a list of store item amount changes to update the store item amounts in the
        // database. There should be no need to group by store item ID since they should already be
        // distinct thanks to validation
        var storeItemAmountChanges = req.StoreTransactionItems
            .Select(sti => new { sti.StoreItemId, sti.ItemAmount, req.StoreId }).ToList();

        // if there is a source store (this is a movement operation), also remove the items from the
        // source store
        if (req.SourceStoreId is { } sourceStoreId) {
            storeItemAmountChanges.AddRange(req.StoreTransactionItems
                    .Select(sti => new { sti.StoreItemId, ItemAmount = -sti.ItemAmount, StoreId = sourceStoreId }));
        }

        // update all the store item amounts in one database call
        _dbContext.StoreItemAmounts
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
        var compositeIds = _dbContext.Compositions
            .Where(c => changedStoreItemIds.Contains(c.StoreItemId))
            .Select(c => c.CompositeId)
            .Distinct()
            .ToArray();

        var compositions = _dbContext.Compositions
            .Where(c => compositeIds.Contains(c.CompositeId))
            .GroupBy(c => c.CompositeId)
            .ToArray();
        var relevantStoreItemIds = compositions.SelectMany(g => g.Select(c => c.StoreItemId)).Distinct();

        List<int> storesToUpdate = [req.StoreId];
        if (req.SourceStoreId is { } id2) {
            storesToUpdate.Add(id2);
        }

        // get store item amounts for all the composites
        var storeItemAmounts = _dbContext.StoreItemAmounts
            .Where(sia => storesToUpdate.Contains(sia.StoreId))
            .Where(sia => relevantStoreItemIds.Contains(sia.StoreItemId))
            .GroupBy(sia => sia.StoreId)
            .Select(g => new {
                StoreId = g.Key,
                ItemAmounts = g.Select(sia => new { sia.StoreItemId, sia.Amount })
            })
            .ToArray();

        List<(int CompositeId, int StoreId, decimal Amount)> newCompositeAmounts = [];

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
        _dbContext.CompositeAmounts
            .Join(
                    newCompositeAmounts,
                    ca => new { ca.StoreId, ca.CompositeId },
                    nc => new { nc.StoreId, nc.CompositeId },
                    (ca, nc) => new { Entity = ca, NewAmount = nc.Amount }
                    )
            .ExecuteUpdate(
                    props => props.SetProperty(
                        x => x.Entity.Amount,
                        x => x.NewAmount
                    ));
    }

}
