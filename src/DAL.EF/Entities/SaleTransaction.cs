namespace KisV4.DAL.EF.Entities;

public record SaleTransaction : Transaction {
    public bool Open { get; set; }

    public int? OpenedById { get; set; }
    public User? OpenedBy { get; set; }

    public ICollection<StoreTransaction> StoreTransactions { get; init; } = [];
    public ICollection<AccountTransaction> AccountTransactions { get; init; } = [];
    public ICollection<SaleTransactionItem> SaleTransactionItems { get; init; } = [];
}
