using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class StoreItemMapping {
    public static StoreItemListModel ToModel(this StoreItem source) =>
        new StoreItemListModel {
            Id = source.Id,
            Name = source.Name,
            CurrentCost = source.CurrentCost,
            IsContainerItem = source.IsContainerItem,
            UnitName = source.UnitName
        };
}
