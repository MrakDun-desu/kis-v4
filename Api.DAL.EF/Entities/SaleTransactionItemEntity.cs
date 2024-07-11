using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a sale transaction item.
/// </summary>
public record SaleTransactionItemEntity {
    public required int Id { get; init; }
    public required int SaleItemId { get; init; }
    public SaleItemEntity? SaleItem { get; set; }
    public required int SaleTransactionId { get; init; }
    /// <summary>
    /// Sale transaction that this transaction item is a part of.
    /// </summary>
    public SaleTransactionEntity? SaleTransaction { get; set; }

    /// <summary>
    /// Modifiers applied to the sale item of this sale transaction item.
    /// </summary>
    public ICollection<ModifierEntity> Modifiers { get; init; } = new List<ModifierEntity>();
    /// <summary>
    /// Prices paid for this transaction item with various currencies.
    /// </summary>
    public ICollection<TransactionPriceEntity> TransactionPrices { get; init; } =
        new List<TransactionPriceEntity>();
    /// <summary>
    /// Discount usage items that have been used on this sale transaction item.
    /// </summary>
    public ICollection<DiscountUsageItemEntity> DiscountUsageItems { get; init; } =
        new List<DiscountUsageItemEntity>();
    /// <summary>
    /// Amount of a sale item that has been sold in this transaction.
    /// </summary>
    public int ItemAmount { get; set; }
}
