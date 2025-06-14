namespace KisV4.Common.Models;

public record CashBoxCreateModel(string Name);

public record CashBoxListModel(int Id, string Name, bool Deleted);

public record CashBoxDetailModel(
    int Id,
    string Name,
    bool Deleted,
    Page<CurrencyChangeListModel> CurrencyChanges,
    IEnumerable<TotalCurrencyChangeListModel>? TotalCurrencyChanges = null,
    IEnumerable<StockTakingModel>? StockTakings = null
) {
    public IEnumerable<TotalCurrencyChangeListModel> TotalCurrencyChanges { get; init; } =
        TotalCurrencyChanges ?? [];

    public IEnumerable<StockTakingModel> StockTakings { get; init; } =
        StockTakings ?? [];
}

public record StockTakingModel(
    DateTimeOffset Timestamp
);
