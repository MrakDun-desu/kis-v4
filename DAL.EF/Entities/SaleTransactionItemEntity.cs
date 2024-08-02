using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a sale transaction item.
/// </summary>
public record SaleTransactionItemEntity {
    public int Id { get; init; }
    public int SaleItemId { get; init; }
    public virtual SaleItemEntity? SaleItem { get; set; }
    public int SaleTransactionId { get; init; }
    /// <summary>
    /// Sale transaction that this transaction item is a part of.
    /// </summary>
    public virtual SaleTransactionEntity? SaleTransaction { get; set; }

    /// <summary>
    /// Modifiers applied to the sale item of this sale transaction item.
    /// </summary>
    public virtual ICollection<ModifierEntity> Modifiers { get; private set; } =
        new List<ModifierEntity>();

    /// <summary>
    /// Prices paid for this transaction item with various currencies.
    /// </summary>
    public virtual ICollection<TransactionPriceEntity> TransactionPrices { get; private set; } =
        new List<TransactionPriceEntity>();

    /// <summary>
    /// Discount usage items that have been used on this sale transaction item.
    /// </summary>
    public virtual ICollection<DiscountUsageItemEntity> DiscountUsageItems { get; private set; } =
        new List<DiscountUsageItemEntity>();
    /// <summary>
    /// Amount of a sale item that has been sold in this transaction.
    /// </summary>
    public int ItemAmount { get; set; }
}
