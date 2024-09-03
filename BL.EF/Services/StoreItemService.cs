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
        var entity = createModel.ToEntity();
        var insertedEntity = dbContext.StoreItems.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public List<StoreItemListModel> ReadAll()
    {
        return dbContext.StoreItems
            .Where(e => !e.Deleted)
            // including categories to prevent lazy loading from sending ton of db queries
            .Include(e => e.Categories)
            .ToList().ToModels();
    }

    public StoreItemDetailModel? Read(int id)
    {
        return dbContext.StoreItems.Find(id).ToModel();
    }

    public bool Update(int id, StoreItemCreateModel updateModel)
    {
        if (dbContext.StoreItems.Any(si => si.Id == id))
            return false;

        var entity = updateModel.ToEntity();
        entity.Id = id;

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