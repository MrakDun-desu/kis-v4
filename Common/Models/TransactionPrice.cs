namespace KisV4.Common.Models;

public record TransactionPriceModel(
    CurrencyReadAllModel Currency,
    decimal Amount
);
