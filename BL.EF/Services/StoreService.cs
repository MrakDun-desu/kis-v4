using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class StoreService(KisDbContext dbContext) : IStoreService, IScopedService
{
    public int Create(StoreCreateModel createModel)
    {
        var entity = createModel.ToEntity();
        var insertedEntity = dbContext.Stores.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public List<StoreReadAllModel> ReadAll()
    {
        return dbContext.Stores.ToList().ToModels();
    }

    public bool Update(int id, StoreUpdateModel updateModel)
    {
        var entity = dbContext.Stores.Find(id);
        if (entity is null)
            return false;

        if (updateModel.Name is not null) entity.Name = updateModel.Name;

        dbContext.Stores.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        var entity = dbContext.Stores.Find(id);
        if (entity is null) return false;

        dbContext.Stores.Remove(entity);
        dbContext.SaveChanges();

        return true;
    }
}