using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
[PrimaryKey(nameof(ModifierId), nameof(SaleTransactionItemId))]
public record Modification {
    public decimal Amount { get; set; }

    public int ModifierId { get; init; }
    public Modifier? Modifier { get; set; }
    public int SaleTransactionItemId { get; init; }
    public SaleTransactionItem? SaleTransactionItem { get; set; }
}
