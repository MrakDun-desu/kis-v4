using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class PipeService(KisDbContext dbContext) : IPipeService, IScopedService {
    public PipeListModel Create(PipeCreateModel createModel) {
        var entity = createModel.ToEntity();
        dbContext.Pipes.Add(entity);

        dbContext.SaveChanges();

        return entity.ToModel();
    }

    public List<PipeListModel> ReadAll() {
        return dbContext.Pipes.ToList().ToModels();
    }

    public OneOf<PipeListModel, NotFound> Update(int id, PipeCreateModel updateModel) {
        var entity = dbContext.Pipes.Find(id);
        if (entity is null) {
            return new NotFound();
        }

        updateModel.UpdateEntity(entity);

        dbContext.Pipes.Update(entity);
        dbContext.SaveChanges();

        return entity.ToModel();
    }

    public OneOf<Success, NotFound, string> Delete(int id) {
        var entity = dbContext.Pipes.Find(id);
        if (entity is null) {
            return new NotFound();
        }

        if (dbContext.Containers.Any(ct => ct.PipeId == id)) {
            return $"Pipe with id {id} cannot be deleted, currently has a " +
                   $"container active";
        }
        dbContext.Pipes.Remove(entity);
        dbContext.SaveChanges();

        return new Success();
    }
}
