using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[PrimaryKey(nameof(StoreId), nameof(CompositeId))]
[AuditIgnore]
public record CompositeAmount {
    public int Amount { get; set; }

    public int CompositeId { get; init; }
    public Composite? Composite { get; set; }
    public int StoreId { get; init; }
    public Store? Store { get; set; }
}
