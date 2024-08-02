using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

public class DiscountService(KisDbContext dbContext, Mapper mapper)
    : IDiscountService, IScopedService {
    public DiscountReadModel? Read(int id) {
        return mapper.ToModel(dbContext.Discounts.Find(id));
    }

    public List<DiscountReadAllModel> ReadAll() {
        return mapper.ToModels(dbContext.Discounts.ToList());
    }
}
