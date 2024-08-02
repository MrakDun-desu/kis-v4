using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

public class CostService(KisDbContext dbContext, Mapper mapper)
    : ICostService, IScopedService {
    public int Create(CostCreateModel createModel) {
        var entity = mapper.ToEntity(createModel with {
            ValidSince = createModel.ValidSince.ToUniversalTime()
        });
        var insertedEntity = dbContext.CurrencyCosts.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }
}
