using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a stock taking for a cashbox.
/// </summary>
[PrimaryKey(nameof(Timestamp))]
public record StockTakingEntity {
    public DateTime Timestamp { get; set; }
}
