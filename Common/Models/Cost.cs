namespace KisV4.Common.Models;

public record CostModel(
    int Id,
    int ProductId,
    int CurrencyId,
    CurrencyReadAllModel Currency,
    DateTimeOffset ValidSince,
    decimal Amount,
    string Description
);
