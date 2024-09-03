using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

public class CostService(KisDbContext dbContext)
    : ICostService, IScopedService
{
    public CostListModel Create(CostCreateModel createModel)
    {
        var entity = (createModel with
        {
            ValidSince = createModel.ValidSince.ToUniversalTime()
        }).ToEntity();
        dbContext.CurrencyCosts.Add(entity);

        dbContext.SaveChanges();

        return entity.ToModel();
    }
}