using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class TapMapping {
    public static TapListModel? ToModel(this Tap? source) => source switch {
        null => null,
        var val => new TapListModel {
            Id = val.Id,
            Name = val.Name,
            ContainerId = val.ContainerId
        }
    };
}
