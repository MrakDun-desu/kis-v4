namespace KisV4.Common.Models;

public record TransactionPriceListModel(
    CurrencyListModel Currency,
    int SaleTransactionItemId,
    decimal Amount,
    bool Cancelled
);