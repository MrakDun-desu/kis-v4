using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents an incomplete transaction that is tracked by the system, but not yet paid for.
/// Also includes the user that the transaction is registered to.
/// </summary>
[PrimaryKey(nameof(SaleTransactionId), nameof(UserId))]
public record IncompleteTransactionEntity {
    public required int SaleTransactionId { get; init; }
    public SaleTransactionEntity? SaleTransaction { get; set; }
    public required int UserId { get; init; }
    public UserAccountEntity? User { get; set; }
}
