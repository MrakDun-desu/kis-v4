using KisV4.BL.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models.CashBox;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class CashboxService(KisDbContext dbContext, Mapper mapper) : ICashboxService, IScopedService {

    public int Create(CashboxCreateModel createModel) {
        var entity = mapper.ToEntity(createModel);
        var insertedEntity = dbContext.CashBoxes.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public List<CashboxListModel> ReadAll() {
        return mapper.ToModelEnumerable(dbContext.CashBoxes).ToList();
    }

    public CashBoxDetailModel? Read(int id) {
        return mapper.ToModel(dbContext.CashBoxes.Find(id));
    }

    public bool Update(int id, CashboxUpdateModel updateModel) {
        var entity = dbContext.CashBoxes.Find(id);
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

        dbContext.CashBoxes.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id) {
        var entity = dbContext.CashBoxes.Find(id);
        if (entity is null) {
            return true;
        }

        dbContext.CashBoxes.Remove(entity);
        dbContext.SaveChanges();

        return true;
    }
}
