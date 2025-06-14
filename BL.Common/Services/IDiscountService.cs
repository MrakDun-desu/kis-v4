using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IDiscountService {
    IEnumerable<DiscountListModel> ReadAll(bool? deleted);
    OneOf<DiscountDetailModel, NotFound> Read(int id);
    OneOf<DiscountDetailModel, NotFound> Patch(int id);
    OneOf<DiscountDetailModel, NotFound> Delete(int id);
}
