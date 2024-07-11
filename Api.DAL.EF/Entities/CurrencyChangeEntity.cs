using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a change in currency that occured in a given account.
/// </summary>
[PrimaryKey(nameof(CurrencyId), nameof(SaleTransactionId), nameof(AccountId))]
public record CurrencyChangeEntity {
    public required int CurrencyId { get; init; }
    /// <summary>
    /// Currency which amount was changed.
    /// </summary>
    public CurrencyEntity? Currency { get; set; }
    public required int SaleTransactionId { get; init; }
    /// <summary>
    /// Sale transaction that has changed the currency amount.
    /// </summary>
    public SaleTransactionEntity? SaleTransaction { get; set; }
    public required int AccountId { get; init; }
    /// <summary>
    /// Account for which the currency amount has changed.
    /// </summary>
    public AccountEntity? Account { get; set; }

    [Precision(11,2)]
    public decimal Amount { get; set; }
}
