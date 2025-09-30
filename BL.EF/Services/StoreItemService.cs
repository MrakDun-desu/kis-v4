using KisV4.BL.Common.Services;
using KisV4.BL.EF.Helpers;
using KisV4.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class StoreItemService(KisDbContext dbContext)
    : IStoreItemService, IScopedService {
    public OneOf<Page<StoreItemListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        bool? deleted,
        int? categoryId,
        int? storeId) {
        var errors = new Dictionary<string, string[]>();
        if (storeId.HasValue && !dbContext.Stores.Any(s => s.Id == storeId)) {
            errors.AddItemOrCreate(
                nameof(storeId),
                $"Store with id {storeId} doesn't exist"
            );
        }

        if (categoryId.HasValue && !dbContext.ProductCategories.Any(cat => cat.Id == categoryId)) {
            errors.AddItemOrCreate(
                nameof(categoryId),
                $"Category with id {categoryId} doesn't exist"
            );
        }

        if (errors.Count > 0) {
            return errors;
        }

        IQueryable<StoreItemEntity> query;
        if (storeId.HasValue) {
            var partQuery = dbContext.StoreTransactionItems
                .Include(sti => sti.StoreItem)
                .Where(sti => sti.StoreId == storeId)
                .Where(sti => !sti.Cancelled);

            if (deleted.HasValue) {
                partQuery = partQuery.Where(sti => sti.StoreItem!.Deleted == deleted.Value);
            }

            if (categoryId.HasValue) {
                partQuery = partQuery
                    .Include(sti => sti.StoreItem)
                    .ThenInclude(si => si!.Categories)
                    .Where(sti => sti.StoreItem!.Categories.Any(cat => cat.Id == categoryId.Value));
            }

            query = partQuery.GroupBy(sti => sti.StoreItem)
                .Select(g =>
                    new {
                        StoreItem = g.Key,
                        Amount = g.Sum(sti => sti.ItemAmount)
                    })
                .Where(item => item.Amount > 0)
                .Select(item => item.StoreItem)!;
        } else {
            query = dbContext.StoreItems.AsQueryable();
            if (deleted.HasValue) {
                query = query.Where(si => si.Deleted == deleted.Value);
            }

            if (categoryId.HasValue) {
                query = query
                    .Include(si => si.Categories)
                    .Where(si => si.Categories.Any(cat => cat.Id == categoryId));
            }
        }

        return query.Page(page ?? 1, pageSize ?? Constants.DefaultPageSize, Mapper.ToModels);
    }

    public OneOf<StoreItemDetailModel, Dictionary<string, string[]>> Create(
        StoreItemCreateModel createModel
    ) {
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

        dbContext.StoreItems.Add(entity);
        dbContext.SaveChanges();

        return new StoreItemIntermediateModel(entity, [], []).ToModel();
    }

    public OneOf<StoreItemDetailModel, NotFound> Read(int id) {
        var entity = dbContext.StoreItems
            .Include(si => si.Categories)
            .Include(si => si.Costs)
            .SingleOrDefault(si => si.Id == id);
        if (entity is null) {
            return new NotFound();
        }

        var currentCosts = dbContext.CurrencyCosts
            .Include(cc => cc.Currency)
            .Where(cc => cc.ProductId == id)
            .GroupBy(cc => cc.Currency)
            .Select(g => g.OrderByDescending(cc => cc.ValidSince).First())
            .ToList().ToModels();

        var storeAmounts = dbContext.StoreTransactionItems
            .Include(sti => sti.Store)
            .Where(sti => sti.StoreItemId == id)
            .Where(sti => !sti.Cancelled)
            .GroupBy(sti => sti.Store)
            .Select(g => new StoreAmountStoreItemListModel(
                g.Key!.ToListModel(), id, g.Sum(sti => sti.ItemAmount)))
            .ToList();

        return new StoreItemIntermediateModel(entity, currentCosts, storeAmounts).ToModel();
    }

    public OneOf<StoreItemDetailModel, NotFound, Dictionary<string, string[]>> Update(
        int id,
        StoreItemCreateModel updateModel
    ) {
        var entity = dbContext.StoreItems
            .Include(si => si.Categories)
            .SingleOrDefault(si => si.Id == id);
        if (entity is null) {
            return new NotFound();
        }

        var categoryIds = updateModel.CategoryIds.Distinct();
        var newCategories = dbContext.ProductCategories.Where(cat => categoryIds.Contains(cat.Id)).ToList();
        var errors = new Dictionary<string, string[]>();
        if (newCategories.Count != categoryIds.Count()) {
            errors.AddItemOrCreate(
                nameof(updateModel.CategoryIds),
                "Some of the submitted categories do not exist"
            );
        }

        if (updateModel.IsContainerItem != entity.IsContainerItem) {
            if (dbContext.ContainerTemplates.Any(ct => ct.ContainedItemId == id)) {
                errors.AddItemOrCreate(
                    nameof(updateModel.IsContainerItem),
                    "Cannot change whether store item is a container item - item already " +
                    "has container templates associated with it"
                    );
            }

            if (dbContext.StoreTransactionItems.Any(sti => sti.StoreItemId == id)) {
                errors.AddItemOrCreate(
                    nameof(updateModel.IsContainerItem),
                    "Cannot change whether store item is a container item - item already " +
                    "has transactions associated with it"
                    );
            }
        }

        if (errors.Count > 0) {
            return errors;
        }

        updateModel.UpdateEntity(entity);
        entity.Categories.Clear();
        foreach (var category in newCategories) {
            entity.Categories.Add(category);
        }

        entity.Deleted = false;

        dbContext.StoreItems.Update(entity);
        dbContext.SaveChanges();

        return Read(entity.Id).AsT0;
    }

    public OneOf<StoreItemDetailModel, NotFound> Delete(int id) {
        var storeItem = dbContext.StoreItems.Find(id);
        if (storeItem is null) {
            return new NotFound();
        }

        storeItem.Deleted = true;
        dbContext.StoreItems.Update(storeItem);
        dbContext.SaveChanges();

        return Read(storeItem.Id).AsT0;
    }
}
