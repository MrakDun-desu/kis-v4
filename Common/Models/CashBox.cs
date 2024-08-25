namespace KisV4.Common.Models;

public record CashBoxCreateModel(string Name);

public record CashBoxUpdateModel(int Id, string Name);

public record CashBoxReadAllModel(int Id, string Name, bool Deleted);

public record CashBoxReadModel(
    int Id,
    string Name,
    bool Deleted,
    ICollection<CurrencyChangeModel> CurrencyChanges,
    ICollection<TotalCurrencyChangeModel>? TotalCurrencyChanges = null,
    ICollection<StockTakingModel>? StockTakings = null
)
{
    public ICollection<TotalCurrencyChangeModel> TotalCurrencyChanges { get; init; } =
        TotalCurrencyChanges ?? new List<TotalCurrencyChangeModel>();

    public ICollection<StockTakingModel> StockTakings { get; init; } =
        StockTakings ?? new List<StockTakingModel>();
}

public record StockTakingModel(
    DateTimeOffset Timestamp
);