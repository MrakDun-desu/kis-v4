namespace KisV4.DAL.EF.Entities;

public record StoreTransaction : Transaction {
    public int? SaleTransactionId { get; init; }
    public required SaleTransaction? SaleTransaction { get; set; }
    public required ICollection<StoreTransactionItem> StoreTransactionItems { get; init; }
}
