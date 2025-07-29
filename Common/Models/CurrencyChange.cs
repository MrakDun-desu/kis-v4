namespace KisV4.Common.Models;

public record CurrencyChangeCreateModel(
    int CurrencyId,
    decimal Amount,
    int AccountId
);

public record CurrencyChangeListModel(
    CurrencyListModel Currency,
    decimal Amount,
    bool Cancelled,
    int SaleTransactionId,
    int AccountId
);

public record TotalCurrencyChangeListModel(
    CurrencyListModel Currency,
    decimal Amount
);
