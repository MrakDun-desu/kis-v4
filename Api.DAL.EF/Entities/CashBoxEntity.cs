namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a cash box in one of the stores.
/// </summary>
public record CashBoxEntity : AccountEntity {
    /// <summary>
    /// Stock-taking periods that have been marked for this cash box.
    /// </summary>
    public ICollection<StockTakingEntity> StockTakings { get; init; } = new List<StockTakingEntity>();
}
