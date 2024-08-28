using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface ICostService
{
    public CostReadAllModel Create(CostCreateModel createModel);
}