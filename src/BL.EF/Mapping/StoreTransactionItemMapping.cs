using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class StoreTransactionItemMapping {
    public static StoreTransactionItemModel ToModel(this StoreTransactionItem source) =>
        new StoreTransactionItemModel {
            StoreTransactionId = source.StoreTransactionId,
            Cost = source.Cost,
            ItemAmount = source.ItemAmount,
            Store = source.Store!.ToModel(),
            StoreItem = source.StoreItem!.ToModel()
        };
}
