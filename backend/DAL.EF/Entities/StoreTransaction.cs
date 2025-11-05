namespace KisV4.DAL.EF.Entities;

public record StoreTransaction : Transaction {
    public StoreTransactionReason Reason { get; init; }

    public int SaleTransactionId { get; init; }
    public SaleTransaction? SaleTransaction { get; set; }
    public ICollection<StoreTransactionItem> StoreTransactionItems { get; init; } = [];
}

public enum StoreTransactionReason {
    AddingToStore = 0,
    ChangingStores,
    WriteOff,
    Sale
}
