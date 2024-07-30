namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a user account.
/// </summary>
public record UserAccountEntity : AccountEntity {
    /// <summary>
    /// Transactions that this user is responsible for.
    /// </summary>
    public virtual ICollection<TransactionEntity> Transactions { get; private set; } =
        new List<TransactionEntity>();

    /// <summary>
    /// Incomplete transactions that this user is supposed to pay for when they're finished.
    /// </summary>
    public virtual ICollection<IncompleteTransactionEntity> IncompleteTransactions {
        get;
        private set;
    } =
        new List<IncompleteTransactionEntity>();

    /// <summary>
    /// Discount usages that this user has applied.
    /// </summary>
    public virtual ICollection<DiscountUsageEntity> DiscountUsages { get; private set; } =
        new List<DiscountUsageEntity>();
}
