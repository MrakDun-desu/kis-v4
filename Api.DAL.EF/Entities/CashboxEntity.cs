namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a cashbox in one of the stores.
/// </summary>
public record CashboxEntity : AccountEntity {
    /// <summary>
    /// Stock-taking periods that have been marked for this cashbox.
    /// </summary>
    public ICollection<StockTakingEntity> StockTakings { get; init; } = new List<StockTakingEntity>();
}
