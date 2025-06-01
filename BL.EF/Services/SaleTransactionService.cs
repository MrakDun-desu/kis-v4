using KisV4.BL.Common.Services;
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
) : ISaleTransactionService {
    public OneOf<SaleTransactionDetailModel, Dictionary<string, string[]>> Create(
        SaleTransactionCreateModel createModel,
        string userName
    ) {
        var errors = ValidateCreateModel(createModel, out var saleItems, out var modifiers);
        if (errors.Count > 0) {
            return errors;
        }

        var responsibleUserId = userService.CreateOrGetId(userName);
        var timestamp = timeProvider.GetUtcNow();
        var newSaleTransaction = new SaleTransactionEntity {
            Timestamp = timestamp,
            ResponsibleUserId = responsibleUserId,
        };

        foreach (var item in createModel.SaleTransactionItems) {
            var newItem = new SaleTransactionItemEntity {
                ItemAmount = item.ItemAmount,
                SaleItemId = item.SaleItemId,
            };
            foreach (ModifierAmountCreateModel modifier in item.ModifierAmounts) {
                newItem.ModifierAmounts.Add(
                    new ModifierAmountEntity {
                        ModifierId = modifier.ModifierId,
                        Amount = modifier.Amount,
                    }
                );
            }
            newSaleTransaction.SaleTransactionItems.Add(newItem);
        }

        // add the sale transaction itself
        dbContext.SaleTransactions.Add(newSaleTransaction);
        // add the incomplete transaction record
        dbContext.IncompleteTransactions.Add(
            new IncompleteTransactionEntity {
                UserId = responsibleUserId,
                SaleTransaction = newSaleTransaction,
            }
        );

        // add the store transactions for the included items
        var newStoreTransactions = new Dictionary<int, StoreTransactionEntity>();
        foreach (var item in createModel.SaleTransactionItems) {
            // for normal store items, it's necessary just to know how much to take out
            // of the given store
            var storeItemAmounts = new Dictionary<int, decimal>();

            // for the container items, it's necessary to know which container to take from,
            // and also how much
            var containerItemAmounts = new Dictionary<int, Dictionary<int, decimal>>();
        }
        var newStoreTransaction = new StoreTransactionEntity {
            Timestamp = timestamp,
            ResponsibleUserId = responsibleUserId,
            TransactionReason = KisV4.Common.Enums.TransactionReason.Sale,
            SaleTransaction = newSaleTransaction,
        };
        dbContext.SaveChanges();
        return Read(newSaleTransaction.Id).AsT0;
    }

    public Dictionary<string, string[]> ValidateCreateModel(
        SaleTransactionCreateModel createModel,
        out Dictionary<int, SaleItemEntity> saleItems,
        out Dictionary<int, ModifierEntity> modifiers
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
            .SaleTransactionItems.SelectMany(sti =>
                sti.ModifierAmounts.Select(mod => mod.ModifierId)
            )
            .Distinct()
            .ToArray();

        var compositionParents = saleItemIds.Concat(modifierIds).ToArray();

        // making requests for the actual items to minimize the amount of DB calls
        // these items will be the passed down to the create/patch method to be used
        saleItems = dbContext
            .SaleItems.Where(si => saleItemIds.Contains(si.Id))
            .ToDictionary(item => item.Id);
        modifiers = dbContext
            .Modifiers.Where(m => modifierIds.Contains(m.Id))
            .ToDictionary(item => item.Id);
        var compositions = dbContext
            .Compositions.Where(c => compositionParents.Contains(c.SaleItemId))
            .GroupBy(c => c.SaleItemId)
            .ToDictionary(g => g.Key, g => g.Select(c => c.StoreItemId).ToArray());

        var storeItemIds = compositions.Values.SelectMany(i => i);
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
                // all requested stores must exist
                if (!stores.TryGetValue(storeId, out var store)) {
                    errors.AddItemOrCreate(nameof(item.StoreId), $"Store {storeId} doesn't exist");
                } else {
                    // we go over all the store items that this transaction item has
                    var containedStoreItems = new HashSet<int>();
                    if (compositions.TryGetValue(item.SaleItemId, out var composition)) {
                        containedStoreItems = [.. containedStoreItems, .. composition];
                    }
                    foreach (var mod in item.ModifierAmounts) {
                        if (!modifiers.ContainsKey(mod.ModifierId)) {
                            continue;
                        }
                        if (compositions.TryGetValue(mod.ModifierId, out composition)) {
                            containedStoreItems = [.. containedStoreItems, .. composition];
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

    public OneOf<SaleTransactionDetailModel, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }

    public OneOf<SaleTransactionDetailModel, NotFound, Dictionary<string, string[]>> Finish(
        int id,
        IEnumerable<CurrencyChangeListModel> currencyChanges
    ) {
        throw new NotImplementedException();
    }

    public OneOf<SaleTransactionDetailModel, Dictionary<string, string[]>> Put(
        IEnumerable<SaleTransactionItemCreateModel> items
    ) {
        throw new NotImplementedException();
    }

    public OneOf<SaleTransactionDetailModel, NotFound> Read(int id) {
        throw new NotImplementedException();
    }

    public List<SaleTransactionListModel> ReadAll(
        int? page,
        int? pageSize,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        bool? cancelled
    ) {
        throw new NotImplementedException();
    }

    public List<SaleTransactionListModel> ReadSelfCancellable() {
        throw new NotImplementedException();
    }
}
