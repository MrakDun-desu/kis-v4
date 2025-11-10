using KisV4.Common.Enums;

namespace KisV4.DAL.EF.Entities;

public record StoreTransaction : Transaction {
    public StoreTransactionReason Reason { get; init; }

    public int? SaleTransactionId { get; init; }
    public SaleTransaction? SaleTransaction { get; set; }
    public ICollection<StoreTransactionItem> StoreTransactionItems { get; init; } = [];
}
