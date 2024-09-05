using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IDiscountService
{
    public IEnumerable<DiscountListModel> ReadAll(bool? deleted);
    public OneOf<DiscountDetailModel, NotFound> Read(int id);
    public OneOf<DiscountDetailModel, NotFound> Patch(int id);
    public OneOf<DiscountDetailModel, NotFound> Delete(int id);
}