using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a partial use of a discount to change the price of a single sale transaction item
/// in a single currency.
/// </summary>
[PrimaryKey(nameof(DiscountUsageId), nameof(CurrencyId), nameof(SaleTransactionItemId))]
public record DiscountUsageItemEntity {
    public required int DiscountUsageId { get; init; }
    public DiscountUsageEntity? DiscountUsage { get; set; }
    public required int CurrencyId { get; init; }
    public CurrencyEntity? Currency { get; set; }
    public required int SaleTransactionItemId { get; init; }
    public SaleTransactionItemEntity? SaleTransactionItem { get; set; }

    [Precision(11,2)]
    public decimal Amount { get; set; }
}
