using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IDiscountUsageService {
    OneOf<Page<DiscountUsageListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        int? discountId,
        int? userId
    );

    OneOf<DiscountUsageDetailModel, Dictionary<string, string[]>> Create(
        DiscountUsageCreateModel createModel
    );

    OneOf<DiscountUsageDetailModel, NotFound> Read(int id);
}
