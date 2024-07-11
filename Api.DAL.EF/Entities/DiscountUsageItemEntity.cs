using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a partial use of a discount to change the price of a single sale transaction item
/// in a single currency.
/// </summary>
[PrimaryKey(nameof(DiscountUsageId), nameof(CurrencyId), nameof(SaleTransactionItemId))]
public record DiscountUsageItemEntity {
    public required int DiscountUsageId { get; init; }
    /// <summary>
    /// Discount usage this has been used as a part of.
    /// </summary>
    public DiscountUsageEntity? DiscountUsage { get; set; }
    public required int CurrencyId { get; init; }
    /// <summary>
    /// Currency for which this discount usage item applies.
    /// </summary>
    public CurrencyEntity? Currency { get; set; }
    public required int SaleTransactionItemId { get; init; }
    /// <summary>
    /// Sale transaction item that this usage item has been used on.
    /// </summary>
    public SaleTransactionItemEntity? SaleTransactionItem { get; set; }

    [Precision(11,2)]
    public decimal Amount { get; set; }
}
