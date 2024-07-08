namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a cashbox in one of the stores.
/// </summary>
public record CashboxEntity : AccountEntity {
    public string Name { get; set; } = string.Empty;
    public bool Deleted { get; set; }
    public ICollection<StockTakingEntity> StockTakings { get; set; } = new List<StockTakingEntity>();
}
