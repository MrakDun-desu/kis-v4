using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class ContainerChangeMapping {
    public static ContainerChangeModel ToModel(this ContainerChange source) =>
        new ContainerChangeModel {
            User = source.User.ToModel()!,
            ContainerId = source.ContainerId,
            NewState = source.NewState,
            Timestamp = source.Timestamp
        };
}
