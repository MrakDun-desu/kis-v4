using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class PipeService(KisDbContext dbContext) : IPipeService, IScopedService
{
    public int Create(PipeCreateModel createModel)
    {
        var entity = createModel.ToEntity();
        var insertedEntity = dbContext.Pipes.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public List<PipeReadAllModel> ReadAll()
    {
        return dbContext.Pipes.ToList().ToModels();
    }

    public bool Update(int id, PipeUpdateModel updateModel)
    {
        var entity = dbContext.Pipes.Find(id);
        if (entity is null)
            return false;

        if (updateModel.Name is not null) entity.Name = updateModel.Name;

        dbContext.Pipes.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        var entity = dbContext.Pipes.Find(id);
        if (entity is null) return false;

        dbContext.Pipes.Remove(entity);
        dbContext.SaveChanges();

        return true;
    }
}