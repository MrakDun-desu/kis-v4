using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class UserMapping {
    public static UserListModel ToModel(this User source) =>
        new UserListModel {
            Id = source.Id
        };
}
