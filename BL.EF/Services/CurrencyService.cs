using KisV4.BL.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class CurrencyService(KisDbContext dbContext, Mapper mapper) : ICurrencyService, IScopedService {

    public int Create(CurrencyCreateModel createModel) {
        var entity = mapper.ToEntity(createModel);
        var insertedEntity = dbContext.Currencies.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public List<CurrencyReadAllModel> ReadAll() {
        return mapper.ToModels(dbContext.Currencies.ToList());
    }

    public bool Update(int id, CurrencyUpdateModel updateModel) {
        var entity = dbContext.Currencies.Find(id);
        if (entity is null)
            return false;

        if (updateModel.Name is not null) {
            entity.Name = updateModel.Name;
        }

        dbContext.Currencies.Update(entity);
        dbContext.SaveChanges();

        return true;
    }
}
