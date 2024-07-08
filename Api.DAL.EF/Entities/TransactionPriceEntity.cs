using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a price of a sale transaction item in a given currency.
/// </summary>
[PrimaryKey(nameof(SaleTransactionItemId), nameof(CurrencyId))]
public record TransactionPriceEntity {
    public required int CurrencyId { get; init; }
    public CurrencyEntity? Currency { get; set; }
    public required int SaleTransactionItemId { get; init; }
    public SaleTransactionItemEntity? SaleTransactionItem { get; set; }

    [Precision(11,2)]
    public decimal Amount { get; set; }
}
