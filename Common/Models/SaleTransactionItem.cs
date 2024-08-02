namespace KisV4.Common.Models;

public record SaleTransactionItemModel(
    int Id,
    SaleItemReadAllModel SaleItem,
    IEnumerable<ModifierReadAllModel> Modifiers,
    IEnumerable<TransactionPriceModel> TransactionPrices,
    int ItemAmount
);