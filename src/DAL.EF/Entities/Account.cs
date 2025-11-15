using Audit.EntityFramework;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
public record Account {
    public int Id { get; init; }
    public ICollection<AccountTransaction> AccountTransactions { get; init; } = [];
}
