using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a store transaction item.
/// </summary>
[PrimaryKey(nameof(StoreItemId), nameof(StoreId))]
public record StoreTransactionItemEntity {
    public required int StoreItemId { get; set; }
    public StoreItemEntity? StoreItem { get; set; }
    public required int StoreId { get; set; }
    public StoreEntity? Store { get; set; }
    /// <summary>
    /// Amount of the item that was added to the store in this transaction.
    /// </summary>
    [Precision(11,2)]
    public decimal ItemAmount { get; set; }
}
