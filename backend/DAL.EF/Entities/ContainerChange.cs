using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
[PrimaryKey(nameof(ContainerId), nameof(Timestamp))]
public record ContainerChange {
    public ContainerState NewState { get; init; }
    public DateTimeOffset Timestamp { get; init; }

    public int ContainerId { get; init; }
    public Container? Container { get; set; }
    public int UserId { get; init; }
    public User? User { get; set; }
}
