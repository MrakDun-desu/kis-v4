using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents a change in currency that occured in a given account.
/// </summary>
[PrimaryKey(nameof(CurrencyId), nameof(SaleTransactionId), nameof(AccountId))]
public record CurrencyChangeEntity
{
    public int CurrencyId { get; init; }

    /// <summary>
    ///     Currency which amount was changed.
    /// </summary>
    public virtual CurrencyEntity? Currency { get; set; }

    public int SaleTransactionId { get; init; }

    /// <summary>
    ///     Sale transaction that has changed the currency amount.
    /// </summary>
    public virtual SaleTransactionEntity? SaleTransaction { get; set; }

    public int AccountId { get; init; }

    /// <summary>
    ///     Account for which the currency amount has changed.
    /// </summary>
    public virtual AccountEntity? Account { get; set; }

    [Precision(11, 2)] public decimal Amount { get; set; }

    public bool Cancelled { get; set; }
}