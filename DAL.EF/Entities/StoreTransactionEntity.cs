using KisV4.Common.Enums;

namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents a store transaction. Includes a reason for transaction and a list of transaction items.
/// </summary>
public record StoreTransactionEntity : TransactionEntity
{
    public TransactionReason TransactionReason { get; set; }
    public int? SaleTransactionId { get; set; }

    /// <summary>
    ///     Sale transaction that has initiated this store transaction, if there is one.
    /// </summary>
    public virtual SaleTransactionEntity? SaleTransaction { get; set; }

    /// <summary>
    ///     Store transaction items that are part of this store transaction.
    /// </summary>
    public virtual ICollection<StoreTransactionItemEntity> StoreTransactionItems { get; private set; } =
        new List<StoreTransactionItemEntity>();
}