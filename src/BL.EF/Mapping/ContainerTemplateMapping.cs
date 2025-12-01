using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class ContainerTemplateMapping {
    public static ContainerTemplateModel ToModel(this ContainerTemplate source) =>
        new ContainerTemplateModel {
            Id = source.Id,
            Name = source.Name,
            Amount = source.Amount,
            StoreItem = source.StoreItem!.ToModel()
        };
}
