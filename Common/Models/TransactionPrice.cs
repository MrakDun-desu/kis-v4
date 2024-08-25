namespace KisV4.Common.Models;

public record TransactionPriceModel(
    CurrencyReadAllModel Currency,
    int SaleTransactionItemId,
    decimal Amount,
    bool Cancelled
);