using Audit.EntityFramework;
using KisV4.Common.Enums;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
public abstract record Account {
    public int Id { get; init; }
    public required AccountType Type { get; init; }

    public ICollection<AccountTransaction> AccountTransactions { get; init; } = [];
}
