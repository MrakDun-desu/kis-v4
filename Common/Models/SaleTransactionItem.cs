namespace KisV4.Common.Models;

public record SaleTransactionItemCreateModel(
    int SaleItemId,
    IEnumerable<ModifierAmountCreateModel> ModifierAmounts,
    int ItemAmount,
    int? StoreId
);

public record SaleTransactionItemListModel(
    int Id,
    SaleItemListModel SaleItem,
    IEnumerable<ModifierAmountListModel> ModifierAmounts,
    IEnumerable<TransactionPriceListModel> TransactionPrices,
    int ItemAmount,
    bool Cancelled
);
