using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IDiscountUsageService
{
    public OneOf<Page<DiscountUsageListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        int? discountId,
        int? userId
    );

    public OneOf<DiscountUsageDetailModel, Dictionary<string, string[]>> Create(DiscountUsageCreateModel createModel);

    public OneOf<DiscountUsageDetailModel, NotFound> Read(int id);
}