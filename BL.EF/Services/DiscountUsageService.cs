using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

public class DiscountUsageService(KisDbContext dbContext) : IDiscountUsageService, IScopedService
{
    public OneOf<List<DiscountUsageListModel>, Dictionary<string, string[]>> ReadAll(int? discountId, int? userId)
    {
        throw new NotImplementedException();
    }

    public OneOf<DiscountUsageDetailModel, Dictionary<string, string[]>> Create(int discountId, int saleTransactionId)
    {
        throw new NotImplementedException();
    }

    public OneOf<DiscountUsageDetailModel, NotFound> Read(int id)
    {
        var output = dbContext.DiscountUsages.Find(id).ToModel();
        if (output is not null) return output;

        return new NotFound();
    }
}