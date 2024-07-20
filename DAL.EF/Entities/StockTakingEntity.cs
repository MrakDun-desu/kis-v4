using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a stock taking for a cashbox.
/// </summary>
public record StockTakingEntity {
    public int Id { get; init; }
    [Precision(0)]
    public DateTime Timestamp { get; set; }
    public required int CashboxId { get; init; }
    public virtual CashBoxEntity? Cashbox { get; set; }
}
