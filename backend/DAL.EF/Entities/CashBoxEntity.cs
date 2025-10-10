namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents a cash box in one of the stores.
/// </summary>
public record CashBoxEntity : AccountEntity {
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Stock-taking periods that have been marked for this cash box.
    /// </summary>
    public virtual ICollection<StockTakingEntity> StockTakings { get; private set; } = [];
}
