using KisV4.Common.Models;
using OneOf;

namespace KisV4.BL.Common.Services;

public interface ICostService {
    OneOf<CostListModel, Dictionary<string, string[]>> Create(CostCreateModel createModel);
}
