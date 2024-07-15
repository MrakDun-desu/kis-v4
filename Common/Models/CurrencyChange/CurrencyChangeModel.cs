using KisV4.Common.Models.Currency;

namespace KisV4.Common.Models.CurrencyChange;

public record CurrencyChangeModel(
    CurrencyModel Currency,
    int SaleTransactionId,
    decimal Amount
);
