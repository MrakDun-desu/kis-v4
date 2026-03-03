using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class ModificationMapping {
    public static ModificationModel ToModel(
        this Modification source
    ) =>
        new() {
            Amount = source.Amount,
            ModifierName = source.Modifier!.Name,
            PriceChange = source.PriceChange
        };

    public static ModificationModel ToModel(
        this Modification source,
        Dictionary<int, (Composite Item, decimal Price)> composites
    ) =>
        new() {
            Amount = source.Amount,
            ModifierName = composites[source.ModifierId].Item.Name,
            PriceChange = source.PriceChange
        };
}
