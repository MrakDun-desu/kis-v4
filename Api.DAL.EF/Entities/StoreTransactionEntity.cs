using Api.DAL.EF.Enums;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a store transaction. Includes a reason for transaction and a list of transaction items.
/// </summary>
public record StoreTransactionEntity : TransactionEntity {
    public TransactionReason TransactionReason { get; set; }
    public int? SaleTransactionId { get; set; }
    public SaleTransactionEntity? SaleTransaction { get; set; }

    public ICollection<StoreTransactionItemEntity> StoreTransactionItems =
        new List<StoreTransactionItemEntity>();
}
