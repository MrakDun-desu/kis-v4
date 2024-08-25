using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents a part of a composition of a sale item.
///     Includes the store item that the sale item is composed of, and its amount.
/// </summary>
[PrimaryKey(nameof(SaleItemId), nameof(StoreItemId))]
public record CompositionEntity
{
    public int SaleItemId { get; init; }

    /// <summary>
    ///     Sale item which is partly composed of given store item.
    /// </summary>
    public virtual SaleItemEntity? SaleItem { get; set; }

    public int StoreItemId { get; init; }

    /// <summary>
    ///     Store item that is a part of the composition of given sale item.
    /// </summary>
    public virtual StoreItemEntity? StoreItem { get; set; }

    [Precision(11, 2)] public decimal Amount { get; set; }
}