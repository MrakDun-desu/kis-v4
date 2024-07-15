namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a sale transaction. Contains sale transaction items, initiated store transactions,
/// and currency changes on accounts that have participated in this transaction.
/// </summary>
public record SaleTransactionEntity : TransactionEntity {
    /// <summary>
    /// Sale transaction items that are part of this sale transaction.
    /// </summary>
    public virtual ICollection<SaleTransactionItemEntity>
        SaleTransactionItems { get; private set; } = new List<SaleTransactionItemEntity>();

    /// <summary>
    /// Store transactions that are initiated by this sale transactions.
    /// </summary>
    public virtual ICollection<StoreTransactionEntity> StoreTransactions { get; private set; } =
        new List<StoreTransactionEntity>();

    /// <summary>
    /// Currency changes on accounts that happened because of this sale transaction.
    /// </summary>
    public virtual ICollection<CurrencyChangeEntity> CurrencyChanges { get; private set; } =
        new List<CurrencyChangeEntity>();
}
