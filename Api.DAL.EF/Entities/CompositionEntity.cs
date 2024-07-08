using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a part of a composition of a sale item.
/// Includes the store item that the sale item is composed of, and its amount.
/// </summary>
[PrimaryKey(nameof(SaleItemId), nameof(StoreItemId))]
public record CompositionEntity {
    public required int SaleItemId { get; set; }
    public SaleItemEntity? SaleItem { get; set; }
    public required int StoreItemId { get; set; }
    public StoreItemEntity? StoreItem { get; set; }

    [Precision(11,2)]
    public decimal Amount { get; set; }
}
