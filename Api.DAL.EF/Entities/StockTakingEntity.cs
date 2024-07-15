namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a stock taking for a cashbox.
/// </summary>
public record StockTakingEntity {
    public required int Id { get; init; }
    public DateTime Timestamp { get; set; }
    public required int CashboxId { get; init; }
    public CashBoxEntity? Cashbox { get; set; }
}
