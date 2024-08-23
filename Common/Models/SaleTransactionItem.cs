namespace KisV4.Common.Models;

public record SaleTransactionItemModel(
    int Id,
    SaleItemReadAllModel SaleItem,
    IEnumerable<ModifierAmountReadAllModel> ModifierAmounts,
    IEnumerable<TransactionPriceModel> TransactionPrices,
    int ItemAmount,
    bool Cancelled
);