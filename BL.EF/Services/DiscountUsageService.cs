using KisV4.BL.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF;

public class DiscountUsageService(KisDbContext dbContext, Mapper mapper) : IDiscountUsageService, IScopedService {
    public DiscountUsageReadModel? Read(int id) {
        return mapper.ToModel(dbContext.DiscountUsages.Find(id));
    }
}
