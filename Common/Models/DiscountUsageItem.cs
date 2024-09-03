namespace KisV4.Common.Models;

public record DiscountUsageItemListModel(
    int DiscountUsageId,
    CurrencyListModel Currency,
    SaleTransactionItemListModel SaleTransactionItem,
    decimal Amount
);