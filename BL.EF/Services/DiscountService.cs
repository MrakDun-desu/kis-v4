using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

public class DiscountService(KisDbContext dbContext)
    : IDiscountService, IScopedService
{
    public List<DiscountListModel> ReadAll()
    {
        return dbContext.Discounts.ToList().ToModels();
    }
}