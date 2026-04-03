using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[PrimaryKey(nameof(AccountId), nameof(SaleTransactionId))]
[AuditIgnore]
public record AccountTransaction {
    public required decimal Amount { get; init; }
    public bool Cancelled { get; set; }

    public required int AccountId { get; init; }
    public Account? Account { get; set; }
    public int SaleTransactionId { get; init; }
    public SaleTransaction? SaleTransaction { get; set; }
}
