using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a partial use of a discount to change the price of a single sale transaction item
/// in a single currency.
/// </summary>
[PrimaryKey(nameof(DiscountUsageId), nameof(CurrencyId), nameof(SaleTransactionItemId))]
public record DiscountUsageItemEntity {
    public int DiscountUsageId { get; init; }
    /// <summary>
    /// Discount usage this has been used as a part of.
    /// </summary>
    public virtual DiscountUsageEntity? DiscountUsage { get; set; }
    public int CurrencyId { get; init; }
    /// <summary>
    /// Currency for which this discount usage item applies.
    /// </summary>
    public virtual CurrencyEntity? Currency { get; set; }
    public int SaleTransactionItemId { get; init; }
    /// <summary>
    /// Sale transaction item that this usage item has been used on.
    /// </summary>
    public virtual SaleTransactionItemEntity? SaleTransactionItem { get; set; }

    [Precision(11,2)]
    public decimal Amount { get; set; }
}
