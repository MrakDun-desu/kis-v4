using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class StoreItemService(KisDbContext dbContext)
    : IStoreItemService, IScopedService
{
    public int Create(StoreItemCreateModel createModel)
    {
        var categories = new List<ProductCategoryEntity>();
        var newCategories = new List<CategoryReadAllModel>();

        foreach (var categoryModel in createModel.Categories)
        {
            var categoryEntity = dbContext.ProductCategories.Find(categoryModel.Id);
            // if category didn't exist, then add a new category to the database
            // otherwise just add the entity itself
            if (categoryEntity is null)
                newCategories.Add(categoryModel with { Id = 0 });
            else
                categories.Add(categoryEntity);
        }

        var entity = (createModel with { Categories = newCategories }).ToEntity();
        var insertedEntity = dbContext.StoreItems.Add(entity);

        foreach (var category in categories) insertedEntity.Entity.Categories.Add(category);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public List<StoreItemReadAllModel> ReadAll()
    {
        return dbContext.StoreItems
            .Where(e => !e.Deleted)
            // including categories to prevent lazy loading from sending ton of db queries
            .Include(e => e.Categories)
            .ToList().ToModels();
    }

    public StoreItemReadModel? Read(int id)
    {
        return dbContext.StoreItems.Find(id).ToModel();
    }

    public bool Update(int id, StoreItemUpdateModel updateModel)
    {
        var entity = dbContext.StoreItems.Find(id);
        if (entity is null)
            return false;

        if (updateModel.Name is not null) entity.Name = updateModel.Name;

        if (updateModel.Image is not null) entity.Image = updateModel.Image;

        if (updateModel.Categories is not null)
        {
            entity.Categories.Clear();
            foreach (var categoryModel in updateModel.Categories)
            {
                var categoryEntity = dbContext.ProductCategories.Find(categoryModel.Id);
                // if category didn't exist, then add a new category to the database
                // otherwise just add the entity itself
                entity.Categories.Add(
                    categoryEntity ??
                    (categoryModel with { Id = 0 }).ToEntity());
            }
        }

        if (updateModel.UnitName is not null) entity.UnitName = updateModel.UnitName;

        if (updateModel.BarmanCanStock.HasValue) entity.BarmanCanStock = updateModel.BarmanCanStock.Value;

        dbContext.StoreItems.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        var entity = dbContext.StoreItems.Find(id);
        if (entity is null) return false;

        entity.Deleted = true;
        dbContext.SaveChanges();

        return true;
    }
}