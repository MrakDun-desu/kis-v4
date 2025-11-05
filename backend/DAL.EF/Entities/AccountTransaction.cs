using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[PrimaryKey(nameof(AccountId), nameof(SaleTransactionId))]
public record AccountTransaction {
    public decimal Amount { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public bool Cancelled { get; set; }

    public int AccountId { get; init; }
    public Account? Account { get; set; }
    public int SaleTransactionId { get; init; }
    public SaleTransaction? SaleTransaction { get; set; }
}
