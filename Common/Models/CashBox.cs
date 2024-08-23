namespace KisV4.Common.Models;

public record CashBoxCreateModel(string Name);

public record CashBoxUpdateModel(string? Name);

public record CashBoxReadAllModel(int Id, string Name, bool Deleted);

public record CashBoxReadModel(
    int Id,
    string Name,
    bool Deleted,
    ICollection<CurrencyChangeModel> CurrencyChanges,
    ICollection<StockTakingModel> StockTakings
);

public record StockTakingModel(
    DateTimeOffset Timestamp
);