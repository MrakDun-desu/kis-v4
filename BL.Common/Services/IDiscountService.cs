using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface IDiscountService
{
    public List<DiscountListModel> ReadAll();
}