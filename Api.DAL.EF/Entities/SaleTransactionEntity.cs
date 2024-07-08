namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a sale transaction. Contains sale transaction items, initiated store transactions,
/// and currency changes on accounts that have participated in this transaction.
/// </summary>
public record SaleTransactionEntity : TransactionEntity {
    public ICollection<SaleTransactionItemEntity> SaleTransactionItems { get; set; } =
        new List<SaleTransactionItemEntity>();

    public ICollection<StoreTransactionEntity> StoreTransactions { get; set; } =
        new List<StoreTransactionEntity>();

    public ICollection<CurrencyChangeEntity> CurrencyChanges { get; set; } =
        new List<CurrencyChangeEntity>();
}
