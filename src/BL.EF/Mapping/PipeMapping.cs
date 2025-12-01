using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class PipeMapping {
    public static PipeListModel? ToModel(this Pipe? source) => source switch {
        null => null,
        var val => new PipeListModel {
            Id = val.Id,
            Name = val.Name
        }
    };
}
