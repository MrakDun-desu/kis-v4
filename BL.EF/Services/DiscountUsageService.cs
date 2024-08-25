using KisV4.BL.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

public class DiscountUsageService(KisDbContext dbContext) : IDiscountUsageService, IScopedService
{
    public DiscountUsageReadModel? Read(int id)
    {
        return dbContext.DiscountUsages.Find(id).ToModel();
    }
}