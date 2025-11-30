using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
[PrimaryKey(nameof(StoreItemId), nameof(StoreId), nameof(StoreTransactionId))]
public record StoreTransactionItem {
    public required decimal ItemAmount { get; set; }
    public required decimal Cost { get; set; }
    public bool Cancelled { get; set; }

    public required int StoreItemId { get; init; }
    public StoreItem? StoreItem { get; set; }
    public required int StoreId { get; init; }
    public Store? Store { get; set; }
    public int StoreTransactionId { get; init; }
    public StoreTransaction? StoreTransaction { get; set; }
}
