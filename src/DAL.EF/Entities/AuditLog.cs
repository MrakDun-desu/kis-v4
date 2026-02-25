using System.Text.Json;
using Audit.EntityFramework;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
public record AuditLog {
    public int Id { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public JsonDocument EntityKeys { get; set; } = null!;
    public string Action { get; set; } = string.Empty;
    public JsonDocument Changes { get; set; } = null!;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }
}
