using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
[PrimaryKey(nameof(StoreItemId), nameof(Timestamp))]
public record Cost {
    public required decimal Amount { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
    public required string Description { get; init; }

    public int StoreItemId { get; init; }
    public StoreItem? StoreItem { get; set; }
    public required int UserId { get; init; }
    public User? User { get; set; }
}
