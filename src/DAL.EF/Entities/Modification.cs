using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
[PrimaryKey(nameof(ModifierId), nameof(SaleTransactionItemLineNumber), nameof(SaleTransactionId))]
public record Modification {
    // amount that this modification was applied on the sale transaction item
    public required int Amount { get; init; }
    // the change in price one modification of this type has on one given sale item
    public required decimal PriceChange { get; init; }

    public required int ModifierId { get; init; }
    public Modifier? Modifier { get; set; }
    public int SaleTransactionItemLineNumber { get; init; }
    public int SaleTransactionId { get; init; }
    public SaleTransactionItem? SaleTransactionItem { get; set; }
}
