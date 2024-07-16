namespace KisV4.Common.Models;

public record CurrencyChangeModel(
    CurrencyReadAllModel Currency,
    int SaleTransactionId,
    decimal Amount
);
