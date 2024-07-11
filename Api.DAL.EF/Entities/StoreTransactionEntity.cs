using Api.DAL.EF.Enums;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a store transaction. Includes a reason for transaction and a list of transaction items.
/// </summary>
public record StoreTransactionEntity : TransactionEntity {
    public TransactionReason TransactionReason { get; set; }
    public int? SaleTransactionId { get; set; }
    /// <summary>
    /// Sale transaction that has initiated this store transaction, if there is one.
    /// </summary>
    public SaleTransactionEntity? SaleTransaction { get; set; }

    /// <summary>
    /// Store transaction items that are part of this store transaction.
    /// </summary>
    public ICollection<StoreTransactionItemEntity> StoreTransactionItems =
        new List<StoreTransactionItemEntity>();
}
