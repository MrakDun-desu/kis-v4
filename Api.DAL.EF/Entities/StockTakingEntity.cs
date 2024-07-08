namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a stock taking for a cashbox.
/// </summary>
public record StockTakingEntity {
    public required int Id { get; init; }
    public DateTime Timestamp { get; set; }
}
