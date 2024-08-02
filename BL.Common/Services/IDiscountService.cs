using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface IDiscountService
{
    public List<DiscountReadAllModel> ReadAll();
    public DiscountReadModel? Read(int id);
}
