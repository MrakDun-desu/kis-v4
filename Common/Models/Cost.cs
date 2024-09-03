namespace KisV4.Common.Models;

public record CostCreateModel(
    int ProductId,
    int CurrencyId,
    DateTimeOffset ValidSince,
    decimal Amount,
    string Description
);

public record CostListModel(
    int Id,
    int ProductId,
    CurrencyListModel Currency,
    DateTimeOffset ValidSince,
    decimal Amount,
    string Description
);

