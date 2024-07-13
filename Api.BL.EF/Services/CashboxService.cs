using Api.BL.Common;
using Api.DAL.EF;
using KisV4.Api.Common.DependencyInjection;
using KisV4.Api.Common.Models.Cashbox;

namespace Api.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class CashboxService(KisDbContext dbContext, Mapper mapper) : ICashboxService, IScopedService {

    public int Create(CashboxCreateModel createModel) {
        var entity = mapper.ToEntity(createModel);
        var insertedEntity = dbContext.Cashboxes.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public List<CashboxListModel> ReadAll() {
        return mapper.ToModelEnumerable(dbContext.Cashboxes).ToList();
    }

    public CashboxDetailModel? Read(int id) {
        return mapper.ToModel(dbContext.Cashboxes.Find(id));
    }

    public bool Update(int id, CashboxUpdateModel updateModel) {
        var entity = dbContext.Cashboxes.Find(id);
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

        dbContext.Cashboxes.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id) {
        var entity = dbContext.Cashboxes.Find(id);
        if (entity is null) {
            return true;
        }

        dbContext.Cashboxes.Remove(entity);
        dbContext.SaveChanges();

        return true;
    }
}
