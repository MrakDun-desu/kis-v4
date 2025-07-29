using KisV4.BL.Common.Services;
using KisV4.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Enums;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

public class SaleTransactionService(
    KisDbContext dbContext,
    IUserService userService,
    TimeProvider timeProvider
) : ISaleTransactionService, IScopedService {

    public OneOf<Page<SaleTransactionListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        bool? cancelled
    ) {
        var query = dbContext.SaleTransactions
            .Include(st => st.ResponsibleUser)
            .OrderByDescending(st => st.Timestamp)
            .AsQueryable();

        if (startDate.HasValue) {
            query = query.Where(sti => sti.Timestamp > startDate.Value);
        }

        if (endDate.HasValue) {
            query = query.Where(sti => sti.Timestamp < endDate.Value);
        }

        if (cancelled.HasValue) {
            query = query.Where(sti => sti.Cancelled == cancelled.Value);
        }

        return query.Page(page ?? 1, pageSize ?? Constants.DefaultPageSize, Mapper.ToModels);
    }

    public IEnumerable<SaleTransactionListModel> ReadSelfCancellable(string userName) {
        var currentTime = timeProvider.GetUtcNow();
        return dbContext.SaleTransactions
            .Include(st => st.ResponsibleUser)
            .Where(st => st.Timestamp > (currentTime - Constants.SelfCancellableTime))
            .Include(st => st.ResponsibleUser)
            .Select(Mapper.ToListModel);
    }

    public OneOf<SaleTransactionDetailModel, Dictionary<string, string[]>> Create(
        SaleTransactionCreateModel createModel,
        string userName
    ) {
        var errors = ValidateCreateModel(createModel, out var compositions);
        if (errors.Count > 0) {
            return errors;
        }

        var newSaleTransaction = new SaleTransactionEntity();
        var responsibleUserId = userService.CreateOrGetId(userName);

        UpdateSaleTransaction(
                createModel,
                newSaleTransaction,
                compositions,
                responsibleUserId,
                out var newStoreTransaction
        );

        // add the sale transaction itself
        dbContext.SaleTransactions.Add(newSaleTransaction);
        // add the incomplete transaction record
        dbContext.IncompleteTransactions.Add(
            new IncompleteTransactionEntity {
                UserId = userService.CreateOrGetId(createModel.ClientUserName),
                SaleTransaction = newSaleTransaction,
            }
        );
        dbContext.StoreTransactions.Add(newStoreTransaction);

        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
        return Read(newSaleTransaction.Id).AsT0;
    }

    public OneOf<SaleTransactionDetailModel, NotFound, Dictionary<string, string[]>> Patch(
        int id,
        SaleTransactionCreateModel updateModel,
        string userName
    ) {
        var saleTransaction = dbContext.SaleTransactions
            .Include(st => st.SaleTransactionItems)
            .ThenInclude(sti => sti.TransactionPrices)
            .SingleOrDefault(st => st.Id == id);
        if (saleTransaction is null) {
            return new NotFound();
        }

        var incompleteTransaction = dbContext.IncompleteTransactions.SingleOrDefault(it => it.SaleTransactionId == id);

        if (incompleteTransaction is null) {
            return new Dictionary<string, string[]> {
                { nameof(id), ["Only incomplete transactions can be updated"] }
            };
        } else if (incompleteTransaction.UserId != userService.CreateOrGetId(updateModel.ClientUserName)) {
            return new Dictionary<string, string[]> {
                {nameof(updateModel.ClientUserName), ["This transaction has been started by a different client"]}
            };
        }

        var errors = ValidateCreateModel(updateModel, out var compositions);
        if (errors.Count > 0) {
            return errors;
        }

        var responsibleUserId = userService.CreateOrGetId(userName);
        UpdateSaleTransaction(
            updateModel,
            saleTransaction,
            compositions,
            responsibleUserId,
            out var newStoreTransaction
        );

        dbContext.SaleTransactions.Update(saleTransaction);
        dbContext.StoreTransactions.Add(newStoreTransaction);

        dbContext.SaveChanges();
        return Read(saleTransaction.Id).AsT0;
    }

    public OneOf<SaleTransactionDetailModel, NotFound, Dictionary<string, string[]>> Finish(
        int id,
        IEnumerable<CurrencyChangeCreateModel> currencyChanges
    ) {
        var requestTime = timeProvider.GetUtcNow();

        var incompleteTransaction = dbContext.IncompleteTransactions
            .Include(it => it.SaleTransaction)
            .SingleOrDefault(it => it.SaleTransactionId == id);

        var saleTransaction = incompleteTransaction?.SaleTransaction ?? dbContext.SaleTransactions.Find(id);

        if (saleTransaction is null) {
            return new NotFound();
        }

        var currencyIds = currencyChanges.Select(cc => cc.CurrencyId).ToArray();
        var accountIds = currencyChanges.Select(cc => cc.AccountId).ToArray();

        var errors = new Dictionary<string, string[]>();

        if (incompleteTransaction is null &&
                requestTime > saleTransaction.Timestamp + Constants.FinishedTransactionChangeTime
                ) {
            errors.AddItemOrCreate(
                    "RequestTime",
                    "Can't change transaction later than " +
                    $"{Constants.FinishedTransactionChangeTime.TotalMinutes} minutes after finishing"
                );
        }

        var realCurrencyIds = dbContext.Currencies.Where(cc => currencyIds.Contains(cc.Id)).Select(cc => cc.Id).ToArray();
        var realAccountIds = dbContext.Accounts.Where(a => accountIds.Contains(a.Id)).Select(a => a.Id).ToArray();

        foreach (var currencyId in currencyIds) {
            if (!realCurrencyIds.Contains(currencyId)) {
                errors.AddItemOrCreate("CurrencyId", $"Currency {currencyId} doesn't exist");
            }
        }
        foreach (var accountId in accountIds) {
            if (!realAccountIds.Contains(accountId)) {
                errors.AddItemOrCreate("AccountId", $"Account {accountId} doesn't exist");
            }
        }

        if (errors.Count != 0) {
            return errors;
        }

        if (incompleteTransaction is not null) {
            dbContext.IncompleteTransactions.Remove(incompleteTransaction);
        }

        // delete old currency changes if any
        dbContext.CurrencyChanges
            .Where(cc => cc.SaleTransactionId == id)
            .ExecuteDelete();

        dbContext.CurrencyChanges.AddRange(currencyChanges.Select(
            cc => new CurrencyChangeEntity {
                AccountId = cc.AccountId,
                Amount = cc.Amount,
                CurrencyId = cc.CurrencyId,
            }
        ));

        dbContext.SaveChanges();

        return Read(saleTransaction.Id).AsT0;
    }

    public OneOf<SaleTransactionDetailModel, NotFound> Read(int id) {
        var output = dbContext.SaleTransactions
            .Include(st => st.SaleTransactionItems)
            .ThenInclude(sti => sti.TransactionPrices)
            .Include(st => st.SaleTransactionItems)
            .ThenInclude(sti => sti.ModifierAmounts)
            .Include(st => st.StoreTransactions)
            .Include(st => st.CurrencyChanges)
            .AsSplitQuery()
            .SingleOrDefault(st => st.Id == id);

        return output is null ? new NotFound() : output.ToModel();
    }

    public OneOf<SaleTransactionDetailModel, NotFound> Delete(int id) {
        var output = dbContext.SaleTransactions
            .Include(st => st.SaleTransactionItems)
            .ThenInclude(sti => sti.TransactionPrices)
            .Include(st => st.SaleTransactionItems)
            .ThenInclude(sti => sti.ModifierAmounts)
            .Include(st => st.StoreTransactions)
            .ThenInclude(st => st.StoreTransactionItems)
            .Include(st => st.CurrencyChanges)
            .AsSplitQuery()
            .SingleOrDefault(st => st.Id == id);

        if (output is null) {
            return new NotFound();
        }

        foreach (var item in output.SaleTransactionItems) {
            item.Cancelled = true;
            foreach (var price in item.TransactionPrices) {
                price.Cancelled = true;
            }
        }
        foreach (var item in output.CurrencyChanges) {
            item.Cancelled = true;
        }
        foreach (var item in output.StoreTransactions) {
            item.Cancelled = true;
            foreach (var transactionItem in item.StoreTransactionItems) {
                transactionItem.Cancelled = true;
            }
        }

        var incompleteTransaction = dbContext.IncompleteTransactions
            .SingleOrDefault(it => it.SaleTransactionId == id);
        if (incompleteTransaction is not null) {
            dbContext.Remove(incompleteTransaction);
        }
        dbContext.Update(output);
        return output.ToModel();
    }

    public Dictionary<string, string[]> ValidateCreateModel(
        SaleTransactionCreateModel createModel,
        out Dictionary<int, CompositionEntity[]> compositions
    ) {
        var errors = new Dictionary<string, string[]>();
        foreach (var item in createModel.SaleTransactionItems) {
            if (item.ItemAmount <= 0) {
                errors.AddItemOrCreate(
                    nameof(item.ItemAmount),
                    $"Sale item {item.SaleItemId} specified invalid amount {item.ItemAmount}. Amount must be more than 0"
                );
            }
        }

        var storeIds = createModel
            .SaleTransactionItems.Where(sti => sti.StoreId.HasValue)
            .Select(sti => sti.StoreId!.Value)
            .Append(createModel.StoreId)
            .Distinct()
            .ToArray();
        var saleItemIds = createModel
            .SaleTransactionItems.Select(sti => sti.SaleItemId)
            .Distinct()
            .ToArray();
        var modifierIds = createModel
            .SaleTransactionItems.SelectMany(sti => sti.ModifierAmounts)
            .Select(mod => mod.ModifierId)
            .Distinct()
            .ToArray();

        var compositionParents = saleItemIds.Concat(modifierIds).ToArray();

        // making requests for the actual items to minimize the amount of DB calls
        // these items will be the passed down to the create/patch method to be used
        var saleItems = dbContext
            .SaleItems.Where(si => saleItemIds.Contains(si.Id))
            .ToDictionary(item => item.Id);
        var modifiers = dbContext
            .Modifiers.Where(m => modifierIds.Contains(m.Id))
            .ToDictionary(item => item.Id);
        compositions = dbContext
            .Compositions.Where(c => compositionParents.Contains(c.SaleItemId))
            .GroupBy(c => c.SaleItemId)
            .ToDictionary(g => g.Key, g => g.ToArray());

        var storeItemIds = compositions.Values.SelectMany(ce => ce.Select(i => i.StoreItemId));
        var storeItemsToIsContainer = dbContext
            .StoreItems.Where(si => storeItemIds.Contains(si.Id))
            .ToDictionary(si => si.Id, si => si.IsContainerItem);

        var stores = dbContext
            .Stores.Where(s => storeIds.Contains(s.Id))
            .ToDictionary(item => item.Id);
        var containersToItems = dbContext
            .Containers.Include(c => c.Template)
            .Where(c => storeIds.Contains(c.Id))
            .ToDictionary(item => item.Id, item => item.Template!.ContainedItemId);

        // default store must exist
        if (!stores.ContainsKey(createModel.StoreId)) {
            errors.AddItemOrCreate(
                nameof(createModel.StoreId),
                $"Store {createModel.StoreId} doesn't exist"
            );
        }
        // default store must not be a container
        if (containersToItems.ContainsKey(createModel.StoreId)) {
            errors.AddItemOrCreate(
                nameof(createModel.StoreId),
                $"Default sale transaction store {createModel.StoreId} cannot be a container"
            );
        }

        foreach (var item in createModel.SaleTransactionItems) {
            // all requested sale items must exist
            if (!saleItems.ContainsKey(item.SaleItemId)) {
                errors.AddItemOrCreate(
                    nameof(createModel.SaleTransactionItems),
                    $"Sale item {item.SaleItemId} doesn't exist"
                );
            }

            foreach (var mod in item.ModifierAmounts) {
                // making sure all modifiers exist
                if (!modifiers.TryGetValue(mod.ModifierId, out var modifier)) {
                    errors.AddItemOrCreate(
                        nameof(item.ModifierAmounts),
                        $"Modifier {mod.ModifierId} doesn't exist"
                    );
                }
                // when modifier exists, also making sure that it's applicable on the given sale item
                else if (modifier.ModificationTargetId != item.SaleItemId) {
                    errors.AddItemOrCreate(
                        nameof(item.ModifierAmounts),
                        $"Modifier {mod.ModifierId} is not a modifier for sale item {item.SaleItemId}"
                    );
                }
            }

            if (item.StoreId is { } storeId) {
                // all requested stores must exist (this checks all the not default stores)
                if (!stores.TryGetValue(storeId, out var store)) {
                    errors.AddItemOrCreate(nameof(item.StoreId), $"Store {storeId} doesn't exist");
                } else {
                    // we go over all the store items that this transaction item has
                    var containedStoreItems = new HashSet<int>();
                    if (compositions.TryGetValue(item.SaleItemId, out var composition)) {
                        containedStoreItems = [.. containedStoreItems, .. composition.Select(c => c.StoreItemId)];
                    }
                    foreach (var mod in item.ModifierAmounts) {
                        if (!modifiers.ContainsKey(mod.ModifierId)) {
                            continue;
                        }
                        if (compositions.TryGetValue(mod.ModifierId, out composition)) {
                            containedStoreItems = [.. containedStoreItems, .. composition.Select(c => c.StoreItemId)];
                        }
                    }

                    // if requested store is a container, it needs to contain correct items
                    if (containersToItems.TryGetValue(storeId, out var containedItemId)) {
                        foreach (var storeItemId in containedStoreItems) {
                            if (containedItemId != storeItemId) {
                                errors.AddItemOrCreate(
                                    nameof(item.StoreId),
                                    $"Container {storeId} isn't used to store item {storeItemId}"
                                );
                            }
                            // just to be sure, check if the store item actually is a container item
                            if (!storeItemsToIsContainer[storeItemId]) {
                                errors.AddItemOrCreate(
                                        nameof(item.StoreId),
                                        $"Container {storeId} can't store a non-container item {storeItemId}"
                                        );
                            }
                        }
                    } else {
                        foreach (var storeItemId in containedStoreItems) {
                            if (storeItemsToIsContainer[storeItemId]) {
                                errors.AddItemOrCreate(
                                        nameof(item.StoreId),
                                        $"The store {storeId} can't store a container item {storeItemId}"
                                        );
                            }
                        }
                    }
                }
            }
        }

        return errors;
    }

    // extracted this into a method since the logic is shared between create and patch
    private void UpdateSaleTransaction(
            SaleTransactionCreateModel updateModel,
            SaleTransactionEntity saleTransaction,
            Dictionary<int, CompositionEntity[]> compositions,
            int responsibleUserId,
            out StoreTransactionEntity storeTransaction
            ) {
        var timestamp = timeProvider.GetUtcNow();

        // first get the prices of everything so it's possible to compute what they should be in the
        // sale transaction items
        var saleItemIds = updateModel.SaleTransactionItems
            .Select(sti => sti.SaleItemId)
            .Distinct();
        var modifierIds = updateModel.SaleTransactionItems
            .SelectMany(sti => sti.ModifierAmounts)
            .Select(ma => ma.ModifierId)
            .Distinct();

        var priceItemIds = saleItemIds.Concat(modifierIds);

        var currentCosts = dbContext.CurrencyCosts
            .Where(cc => priceItemIds.Contains(cc.ProductId))
            .Where(cc => cc.ValidSince < timestamp)
            .GroupBy(cc => cc.ProductId)
            .Select(g => new {
                ProductId = g.Key,
                CurrentCosts = g.GroupBy(cc => cc.CurrencyId)
                    .Select(gg => new {
                        CurrencyId = gg.Key,
                        CurrentCost = gg.OrderByDescending(cc => cc.ValidSince)
                                .First().Amount
                    })
            })
        .ToArray()
        .ToDictionary(i => i.ProductId, i => i.CurrentCosts);

        // update the sale transaction (mainly prices)
        // old timestamp and user still stay in the store transactions that have been done
        // previously (user honestly shouldn't change anyways)
        saleTransaction.ResponsibleUserId = responsibleUserId;
        saleTransaction.Timestamp = timestamp;
        foreach (var item in updateModel.SaleTransactionItems) {
            var newStItem = new SaleTransactionItemEntity {
                ItemAmount = item.ItemAmount,
                SaleItemId = item.SaleItemId,
            };
            addToTransactionPrice(item.SaleItemId, item.ItemAmount);
            foreach (var modifier in item.ModifierAmounts) {
                newStItem.ModifierAmounts.Add(
                    new ModifierAmountEntity {
                        ModifierId = modifier.ModifierId,
                        Amount = modifier.Amount,
                    }
                );
                addToTransactionPrice(modifier.ModifierId, modifier.Amount);
            }
            saleTransaction.SaleTransactionItems.Add(newStItem);

            void addToTransactionPrice(int productId, int productAmount) {
                foreach (var currencyCost in currentCosts[item.SaleItemId]) {
                    var currencyId = currencyCost.CurrencyId;
                    var cost = currencyCost.CurrentCost;
                    var priceEntityOpt = newStItem
                        .TransactionPrices
                        .SingleOrDefault(tp => tp.CurrencyId == currencyId);
                    var priceEntity = priceEntityOpt ?? new TransactionPriceEntity {
                        Amount = 0,
                        CurrencyId = currencyId
                    };
                    priceEntity.Amount += cost;
                    if (priceEntityOpt is null) {
                        newStItem.TransactionPrices.Add(priceEntity);
                    }
                }
            }
        }

        // add new store transaction
        storeTransaction = new StoreTransactionEntity {
            ResponsibleUserId = responsibleUserId,
            SaleTransaction = saleTransaction,
            Timestamp = timestamp,
            TransactionReason = TransactionReason.Sale,
        };
        foreach (var saleItem in updateModel.SaleTransactionItems) {
            var storeId = saleItem.StoreId ?? updateModel.StoreId;
            // first add all the store item amounts from the sale item itself
            var storeItemAmounts = compositions[saleItem.SaleItemId]
                .ToDictionary(comp => comp.StoreItemId, comp => comp.Amount * saleItem.ItemAmount);

            // then add all the modifier store item amounts
            foreach (var (modId, modAmount) in saleItem.ModifierAmounts) {
                var modifierComp = compositions[modId];
                foreach (var comp in modifierComp) {
                    var amountModification = comp.Amount * modAmount;
                    if (storeItemAmounts.ContainsKey(comp.StoreItemId)) {
                        storeItemAmounts[comp.StoreItemId] += amountModification;
                    } else {
                        storeItemAmounts[comp.StoreItemId] = amountModification;
                    }
                }
            }

            // add all the new store item amounts to the transaction
            foreach (var itemAmount in storeItemAmounts) {
                var transactionItemOpt = storeTransaction
                    .StoreTransactionItems
                    .SingleOrDefault(sti => sti.StoreItemId == itemAmount.Key && sti.StoreId == storeId);

                // if the item wasn't found (this store item wasn't added yet), create it
                var transactionItem = transactionItemOpt ?? new StoreTransactionItemEntity {
                    StoreId = storeId,
                    StoreItemId = itemAmount.Key,
                    StoreTransaction = storeTransaction,
                    ItemAmount = 0,
                };
                // here it's needed to subtract because while the amounts of the sale items passed
                // are positive, they're being *taken away* from the stores
                transactionItem.ItemAmount -= itemAmount.Value;

                // add the item into the transaction only if it wasn't there yet
                if (transactionItemOpt is null) {
                    storeTransaction.StoreTransactionItems.Add(transactionItem);
                }
            }
        }

        // Maybe add this to check if we're selling a negative amount of a store item (shouldn't
        // ever happen)
        // foreach (var sti in storeTransaction.StoreTransactionItems) {
        //     if (sti.ItemAmount > 0) {
        //         errors.AddItemOrCreate(sti.StoreItem.Name, "Cannot sell a negative amount of store item");
        //     }
        // }
    }
}
