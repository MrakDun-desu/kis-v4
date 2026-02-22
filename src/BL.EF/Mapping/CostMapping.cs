using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class CostMapping {
    public static CostModel ToModel(this Cost source) => new CostModel {
        Amount = source.Amount,
        Description = source.Description,
        Timestamp = source.Timestamp,
        StoreItemId = source.StoreItemId,
        User = source.User.ToModel()!
    };
}
