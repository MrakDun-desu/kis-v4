using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class UserMapping {
    public static UserListModel? ToModel(this User? source) => source switch {
        null => null,
        var val => new UserListModel {
            Id = val.Id
        }
    };
}
