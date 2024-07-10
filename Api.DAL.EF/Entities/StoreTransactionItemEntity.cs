using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a store transaction item.
/// </summary>
public record StoreTransactionItemEntity {
    public required int Id { get; init; }

    public required int StoreItemId { get; init; }
    public StoreItemEntity? StoreItem { get; set; }
    public required int StoreId { get; init; }
    public StoreEntity? Store { get; set; }
    public required int StoreTransactionId { get; init; }
    public StoreTransactionEntity? StoreTransaction { get; set; }

    /// <summary>
    /// Amount of the item that was added to the store in this transaction.
    /// </summary>
    [Precision(11,2)]
    public decimal ItemAmount { get; set; }
}
