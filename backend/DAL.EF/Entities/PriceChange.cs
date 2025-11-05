using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
[PrimaryKey(nameof(DiscountUsageId), nameof(SaleTransactionItemId))]
public record PriceChange {
    public decimal Amount { get; set; }

    public int DiscountUsageId { get; init; }
    public DiscountUsage? DiscountUsage { get; set; }
    public int SaleTransactionItemId { get; init; }
    public SaleTransactionItem? SaleTransactionItem { get; set; }
}
