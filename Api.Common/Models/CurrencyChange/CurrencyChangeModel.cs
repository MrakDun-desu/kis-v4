using KisV4.Api.Common.Models.Currency;

namespace KisV4.Api.Common.Models.CurrencyChange;

public record CurrencyChangeModel(
    CurrencyModel Currency,
    int SaleTransactionId,
    decimal Amount
);
