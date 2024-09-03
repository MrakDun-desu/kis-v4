using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IDiscountUsageService
{
    public OneOf<List<DiscountUsageReadAllModel>, Dictionary<string, string[]>> ReadAll(
        int? discountId,
        int? userId
    );

    public OneOf<DiscountUsageReadModel, Dictionary<string, string[]>> Create(
        int discountId,
        int saleTransactionId
    );

    public OneOf<DiscountUsageReadModel, NotFound> Read(int id);
}