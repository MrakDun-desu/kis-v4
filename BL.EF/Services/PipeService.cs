using KisV4.BL.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class PipeService(KisDbContext dbContext, Mapper mapper) : IPipeService, IScopedService {

    public int Create(PipeCreateModel createModel) {
        var entity = mapper.ToEntity(createModel);
        var insertedEntity = dbContext.Pipes.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public List<PipeModel> ReadAll() {
        return mapper.ToModels(dbContext.Pipes).ToList();
    }

    public bool Update(int id, PipeUpdateModel updateModel) {
        var entity = dbContext.Pipes.Find(id);
        if (entity is null)
            return false;

        var changed = false;

        if (updateModel.Name is not null) {
            entity.Name = updateModel.Name;
            changed = true;
        }

        if (!changed) {
            return true;
        }

        dbContext.Pipes.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id) {
        var entity = dbContext.Pipes.Find(id);
        if (entity is null) {
            return true;
        }

        dbContext.Pipes.Remove(entity);
        dbContext.SaveChanges();

        return true;
    }
}
