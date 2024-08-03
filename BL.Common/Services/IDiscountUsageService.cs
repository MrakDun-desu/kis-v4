using KisV4.Common.Models;

namespace KisV4.BL.Common;

public interface IDiscountUsageService
{
    public DiscountUsageReadModel? Read(int id);
}
