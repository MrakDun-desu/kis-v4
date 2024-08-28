using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

public class DiscountService(KisDbContext dbContext)
    : IDiscountService, IScopedService
{
    public OneOf<DiscountReadModel, NotFound> Read(int id)
    {
        var entity = dbContext.Discounts.Find(id);
        if (entity is null)
        {
            return new NotFound();
        }

        return entity.ToModel();
    }

    public List<DiscountReadAllModel> ReadAll()
    {
        return dbContext.Discounts.ToList().ToModels();
    }
}