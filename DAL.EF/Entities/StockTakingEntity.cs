using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a stock taking for a cash-box.
/// </summary>
[PrimaryKey(nameof(Timestamp), nameof(CashBoxId))]
public record StockTakingEntity {
    [Precision(0)]
    public DateTimeOffset Timestamp { get; set; }
    public int CashBoxId { get; init; }
    public virtual CashBoxEntity? CashBox { get; set; }
}
