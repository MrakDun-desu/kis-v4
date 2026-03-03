using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
[PrimaryKey(nameof(LineNumber), nameof(SaleTransactionId))]
public record SaleTransactionItem {
    // identifier of a sale transaction item within a sale transaction
    public required int LineNumber { get; init; }
    // amount of the item in the transaction
    public required int Amount { get; init; }
    public bool Cancelled { get; set; }
    /// price of one sale item without modifiers
    public required decimal BasePrice { get; init; }

    public int SaleTransactionId { get; init; }
    public SaleTransaction? SaleTransaction { get; set; }
    public required int SaleItemId { get; init; }
    public SaleItem? SaleItem { get; set; }
    public ICollection<Modification> Modifications { get; init; } = [];
    public ICollection<PriceChange> PriceChanges { get; init; } = [];
}
