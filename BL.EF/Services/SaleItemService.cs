using KisV4.BL.Common.Services;
using KisV4.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class SaleItemService(KisDbContext dbContext, TimeProvider timeProvider)
    : ISaleItemService, IScopedService {
    public OneOf<Page<SaleItemListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        bool? deleted,
        int? categoryId,
        bool? showOnWeb) {
        var errors = new Dictionary<string, string[]>();
        // can be left here until filtering by store is implemented
        // if (storeId.HasValue && !dbContext.Stores.Any(s => s.Id == storeId))
        // {
        //     errors.AddItemOrCreate(
        //         nameof(storeId),
        //         $"Store with id {storeId} doesn't exist"
        //     );
        // }

        if (categoryId.HasValue && !dbContext.ProductCategories
                .Any(cat => cat.Id == categoryId)) {
            errors.AddItemOrCreate(
                nameof(categoryId),
                $"Category with id {categoryId} doesn't exist"
            );
        }

        if (errors.Count > 0) {
            return errors;
        }

        var query = dbContext.SaleItems.AsQueryable();
        if (deleted.HasValue) {
            query = query.Where(si => si.Deleted == deleted.Value);
        }

        if (showOnWeb.HasValue) {
            query = query.Where(si => si.Deleted == showOnWeb.Value);
        }

        if (categoryId.HasValue) {
            query = query
                .Include(si => si.Categories)
                .Where(si => si.Categories.Any(cat => cat.Id == categoryId));
        }

        return query.Page(page ?? 1, pageSize ?? Constants.DefaultPageSize, Mapper.ToModels);
    }

    public OneOf<SaleItemDetailModel, Dictionary<string, string[]>> Create(SaleItemCreateModel createModel) {
        var categoryIds = createModel.CategoryIds.Distinct();
        var realCategories = dbContext.ProductCategories.Where(cat => categoryIds.Contains(cat.Id)).ToList();
        if (realCategories.Count != categoryIds.Count()) {
            return new Dictionary<string, string[]>
            {
                {
                    nameof(createModel.CategoryIds),
                    ["Some of the submitted categories do not exist"]
                }
            };
        }

        var entity = createModel.ToEntity();
        foreach (var category in realCategories) {
            entity.Categories.Add(category);
        }

        dbContext.SaleItems.Add(entity);
        dbContext.SaveChanges();

        return new SaleItemIntermediateModel(entity, [], []).ToModel();
    }

    public OneOf<SaleItemDetailModel, NotFound> Read(int id) {
        var entity = dbContext.SaleItems
            .Include(si => si.Composition)
            .SingleOrDefault(st => st.Id == id);

        if (entity is null) {
            return new NotFound();
        }

        var timestamp = timeProvider.GetUtcNow();

        var currentCosts = dbContext.CurrencyCosts
            .Include(cc => cc.Currency)
            .Where(cc => cc.ProductId == id)
            .Where(cc => cc.ValidSince < timestamp)
            .GroupBy(cc => cc.Currency)
            .Select(g => g
                .OrderByDescending(cc => cc.ValidSince)
                .First())
            .ToList().ToModels();

        var composition = entity.Composition
            .ToDictionary(
                comp => comp.StoreItemId,
                comp => comp.Amount
            );

        var storeAmountsDict = dbContext.StoreTransactionItems
            .Where(sti => composition.Keys.Contains(sti.StoreItemId))
            .Where(sti => !sti.Cancelled)
            .Where(sti => !sti.Store!.Deleted)
            .GroupBy(sti => sti.StoreId)
            .Select(g =>
                new {
                    StoreId = g.Key,
                    Amounts = g
                        .GroupBy(sti => sti.StoreItemId)
                        .Select(ing => new {
                            ing.Key,
                            Value = ing.Sum(sti => sti.ItemAmount)
                        })
                }
            )
            .ToDictionary(
                item => item.StoreId,
                item => item.Amounts
            );

        var stores = dbContext.Stores
            .Where(st => !st.Deleted).ToList();
        var storeAmounts = new List<StoreAmountSaleItemListModel>();
        foreach (var store in stores) {
            // if store didn't even have an entry, just add it with amount of 0
            if (storeAmountsDict.TryGetValue(store.Id, out var amounts)) {
                // need to go over all the items in composition to see if some are missing
                // store transaction items for the given store - if there are any, have to set
                // the amount to 0 without checking any of the other item amounts
                if (composition.Any(kvp => !dbContext.StoreTransactionItems
                        .Any(sti => sti.StoreItemId == kvp.Key && sti.StoreId == store.Id))) {
                    storeAmounts.Add(new StoreAmountSaleItemListModel(
                        store.ToListModel(),
                        id,
                        0
                    ));
                }
                // else just take the minimum amount of item counts divided by
                // their composition amount
                else {
                    var amount = Math.Max((int)amounts
                        .Min(kvp => kvp.Value / composition[kvp.Key]), 0);
                    storeAmounts.Add(new StoreAmountSaleItemListModel(
                        store.ToListModel(),
                        id,
                        amount
                    ));
                }
            } else {
                // for all stores that don't have store transaction items at all,
                // just set the store amounts to 0
                storeAmounts.Add(new StoreAmountSaleItemListModel(
                    store.ToListModel(),
                    id,
                    0
                ));
            }
        }


        return new SaleItemIntermediateModel(entity, currentCosts, storeAmounts).ToModel();
    }

    public OneOf<SaleItemDetailModel, NotFound, Dictionary<string, string[]>> Update(int id,
        SaleItemCreateModel updateModel) {
        var entity = dbContext.SaleItems.Find(id);
        if (entity is null) {
            return new NotFound();
        }

        var categoryIds = updateModel.CategoryIds.Distinct();
        var realCategories = dbContext.ProductCategories.Where(cat => categoryIds.Contains(cat.Id)).ToList();
        if (realCategories.Count != categoryIds.Count()) {
            return new Dictionary<string, string[]>
            {
                {
                    nameof(updateModel.CategoryIds),
                    ["Some of the submitted categories do not exist"]
                }
            };
        }

        updateModel.UpdateEntity(entity);
        entity.Deleted = false;
        entity.Categories.Clear();
        foreach (var category in realCategories) {
            entity.Categories.Add(category);
        }

        dbContext.SaleItems.Update(entity);
        dbContext.SaveChanges();

        return Read(id).AsT0;
    }

    public OneOf<SaleItemDetailModel, NotFound> Delete(int id) {
        var entity = dbContext.SaleItems.Find(id);
        if (entity is null) {
            return new NotFound();
        }

        entity.Deleted = true;
        dbContext.SaleItems.Update(entity);
        dbContext.SaveChanges();

        return Read(id).AsT0;
    }
}
