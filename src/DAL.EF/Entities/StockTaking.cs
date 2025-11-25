using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
[PrimaryKey(nameof(CashBoxId), nameof(Timestamp))]
public record StockTaking {
    public required DateTimeOffset Timestamp { get; init; }

    public int CashBoxId { get; init; }
    public Cashbox? CashBox { get; set; }
    public required int UserId { get; init; }
    public User? User { get; set; }
}
