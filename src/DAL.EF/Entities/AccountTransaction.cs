using Audit.EntityFramework;
using KisV4.Common.Enums;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
public record AccountTransaction {
    public int Id { get; init; }
    public required decimal Amount { get; init; }
    public bool Cancelled { get; set; }
    public required AccountTransactionType Type { get; init; }
    public required DateTimeOffset Timestamp { get; init; }

    public required int AccountId { get; init; }
    public Account? Account { get; set; }
    public int? SaleTransactionId { get; init; }
    public SaleTransaction? SaleTransaction { get; set; }
}
