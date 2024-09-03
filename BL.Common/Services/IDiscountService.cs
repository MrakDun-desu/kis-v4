using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IDiscountService
{
    public List<DiscountReadAllModel> ReadAll();
}