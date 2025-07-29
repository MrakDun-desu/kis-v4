using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents a price of a sale transaction item in a given currency.
/// </summary>
[PrimaryKey(nameof(SaleTransactionItemId), nameof(CurrencyId))]
public record TransactionPriceEntity {
    public int CurrencyId { get; init; }

    /// <summary>
    ///     Currency that this transaction price is in.
    /// </summary>
    public virtual CurrencyEntity? Currency { get; set; }

    public int SaleTransactionItemId { get; init; }

    /// <summary>
    ///     Sale transaction item that has this price.
    /// </summary>
    public virtual SaleTransactionItemEntity? SaleTransactionItem { get; set; }

    [Precision(11, 2)] public decimal Amount { get; set; }

    public bool Cancelled { get; set; }
}
