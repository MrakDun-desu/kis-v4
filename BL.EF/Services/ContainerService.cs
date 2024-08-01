using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class ContainerService(KisDbContext dbContext, Mapper mapper, TimeProvider timeProvider) : IContainerService, IScopedService {

    public int Create(ContainerCreateModel createModel) {
        // can't implement this without user login feature
        throw new NotImplementedException();
    }

    public List<ContainerReadAllModel> ReadAll() {
        return mapper.ToModels(dbContext.Containers.Where(c => !c.Deleted).ToList());
    }

    public bool Update(int id, ContainerUpdateModel updateModel) {
        var entity = dbContext.Containers.Find(id);
        if (entity is null || entity.Deleted)
            return false;

        if (updateModel.Name is not null) {
            entity.Name = updateModel.Name;
        }
        
        if (updateModel.PipeId is not null 
            && entity.PipeId is null
            && entity.OpenSince is null) {
            entity.OpenSince = timeProvider.GetUtcNow();
        }

        entity.PipeId = updateModel.PipeId;

        dbContext.Containers.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id) {
        // can't implement this without user login feature
        throw new NotImplementedException();
    }
}
