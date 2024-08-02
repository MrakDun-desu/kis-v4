namespace KisV4.Common.Models;

public record DiscountUsageItemModel(
    CurrencyReadAllModel Currency,
    SaleTransactionItemModel SaleTransactionItem,
    decimal Amount
);
