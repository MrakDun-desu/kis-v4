using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a general transaction.
/// </summary>
public abstract record TransactionEntity {
    public int Id { get; set; }

    public int? ResponsibleUserId { get; set; }
    /// <summary>
    /// User that initiated this transaction in the system. Can be barman for sale transactions and
    /// some store transactions, or inventory manager for other store transactions.
    /// </summary>
    public virtual UserAccountEntity? ResponsibleUser { get; set; }
    [Precision(0)]
    public DateTimeOffset Timestamp { get; set; }
    public bool Cancelled { get; set; }
}
