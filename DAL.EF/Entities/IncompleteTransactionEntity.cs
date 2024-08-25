using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents an incomplete transaction that is tracked by the system, but not yet paid for.
///     Also includes the user that the transaction is registered to.
/// </summary>
[PrimaryKey(nameof(SaleTransactionId), nameof(UserId))]
public record IncompleteTransactionEntity
{
    public int SaleTransactionId { get; init; }

    /// <summary>
    ///     Incomplete sale transaction this record responds to.
    /// </summary>
    public virtual SaleTransactionEntity? SaleTransaction { get; set; }

    public int UserId { get; init; }

    /// <summary>
    ///     User that is supposed to pay for this incomplete transaction when it finishes.
    /// </summary>
    public virtual UserAccountEntity? User { get; set; }
}