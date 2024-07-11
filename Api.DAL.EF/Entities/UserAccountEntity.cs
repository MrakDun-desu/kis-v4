namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a user account.
/// </summary>
public record UserAccountEntity : AccountEntity {
    /// <summary>
    /// Transactions that this user is responsible for.
    /// </summary>
    public ICollection<TransactionEntity> Transactions { get; init; } = new List<TransactionEntity>();

    /// <summary>
    /// Incomplete transactions that this user is supposed to pay for when they're finished.
    /// </summary>
    public ICollection<IncompleteTransactionEntity> IncompleteTransactions { get; init; } =
        new List<IncompleteTransactionEntity>();

    /// <summary>
    /// Discount usages that this user has applied.
    /// </summary>
    public ICollection<DiscountUsageEntity> DiscountUsages { get; init; } =
        new List<DiscountUsageEntity>();
}
