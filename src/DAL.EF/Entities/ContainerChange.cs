using Audit.EntityFramework;
using KisV4.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
[PrimaryKey(nameof(ContainerId), nameof(Timestamp))]
public record ContainerChange {
    public required ContainerState NewState { get; init; }
    public required decimal NewAmount { get; init; }
    public required DateTimeOffset Timestamp { get; init; }

    public required int ContainerId { get; init; }
    public Container? Container { get; set; }
    public required int UserId { get; init; }
    public User? User { get; set; }
}
