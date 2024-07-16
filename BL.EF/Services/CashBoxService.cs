using KisV4.BL.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class CashBoxService(KisDbContext dbContext, Mapper mapper) : ICashBoxService, IScopedService {

    public int Create(CashBoxCreateModel createModel) {
        var entity = mapper.ToEntity(createModel);
        var insertedEntity = dbContext.CashBoxes.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public List<CashBoxReadAllModel> ReadAll() {
        return mapper.ToModels(dbContext.CashBoxes.ToList());
    }

    public CashBoxReadModel? Read(int id) {
        return mapper.ToModel(dbContext.CashBoxes.Find(id));
    }

    public bool Update(int id, CashBoxUpdateModel updateModel) {
        var entity = dbContext.CashBoxes.Find(id);
        if (entity is null)
            return false;

        if (updateModel.Name is not null) {
            entity.Name = updateModel.Name;
        }

        dbContext.CashBoxes.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id) {
        var entity = dbContext.CashBoxes.Find(id);
        if (entity is null) {
            return false;
        }

        entity.Deleted = true;
        dbContext.SaveChanges();

        return true;
    }
}
