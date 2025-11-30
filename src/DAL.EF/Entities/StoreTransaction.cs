using KisV4.Common.Enums;

namespace KisV4.DAL.EF.Entities;

public record StoreTransaction : Transaction {
    public required StoreTransactionReason Reason { get; init; }

    public int? SaleTransactionId { get; init; }
    public required SaleTransaction? SaleTransaction { get; set; }
    public required ICollection<StoreTransactionItem> StoreTransactionItems { get; init; }
}
