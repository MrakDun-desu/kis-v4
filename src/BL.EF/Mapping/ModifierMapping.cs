using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class ModifierMapping {
    public static ModifierListModel ToModel(this Modifier source) =>
        new() {
            Id = source.Id,
            Name = source.Name,
            Image = source.Image,
            MarginPercent = source.MarginPercent,
            MarginStatic = source.MarginStatic,
            PrestigeAmount = source.PrestigeAmount
        };
}
