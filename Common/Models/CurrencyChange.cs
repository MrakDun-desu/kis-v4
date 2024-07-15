using KisV4.Common.Models;

namespace KisV4.Common.Models;

public record CurrencyChangeModel(
    CurrencyModel Currency,
    int SaleTransactionId,
    decimal Amount
);
