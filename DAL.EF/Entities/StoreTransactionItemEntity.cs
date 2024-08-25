using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents a store transaction item.
/// </summary>
public record StoreTransactionItemEntity
{
    public int Id { get; init; }

    public int StoreItemId { get; init; }
    public virtual StoreItemEntity? StoreItem { get; set; }
    public int StoreId { get; init; }

    /// <summary>
    ///     Store where the amount of store item has changed as a part of this store transaction item.
    /// </summary>
    public virtual StoreEntity? Store { get; set; }

    public int StoreTransactionId { get; init; }

    /// <summary>
    ///     Store transaction that this transaction item belongs to.
    /// </summary>
    public virtual StoreTransactionEntity? StoreTransaction { get; set; }

    /// <summary>
    ///     Amount of the item that was added to the store in this transaction.
    /// </summary>
    [Precision(11, 2)]
    public decimal ItemAmount { get; set; }

    public bool Cancelled { get; set; }
}