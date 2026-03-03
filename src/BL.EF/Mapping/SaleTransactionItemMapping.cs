using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class SaleTransactionItemMapping {
    public static SaleTransactionItemModel ToModel(
        this SaleTransactionItem source
    ) => new() {
        Amount = source.Amount,
        LineNumber = source.LineNumber,
        Modifications = source.Modifications.Select(m => m.ToModel()),
        SaleItemName = source.SaleItem!.Name,
        BasePrice = source.BasePrice
    };

    public static SaleTransactionItemModel ToModel(
        this SaleTransactionItem source,
        Dictionary<int, (Composite Item, decimal Price)> composites
    ) => new() {
        Amount = source.Amount,
        LineNumber = source.LineNumber,
        Modifications = source.Modifications.Select(m => m.ToModel(composites)),
        SaleItemName = composites[source.SaleItemId].Item.Name,
        BasePrice = source.BasePrice
    };
}
