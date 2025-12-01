using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class StoreMapping {
    public static StoreListModel ToModel(this Store source) =>
        new StoreListModel {
            Id = source.Id,
            Name = source.Name
        };
}
