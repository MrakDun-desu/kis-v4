using System.ComponentModel.DataAnnotations.Schema;
using Audit.EntityFramework;

namespace KisV4.DAL.EF.Entities;

[AuditIgnore]
public record User {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required string Id { get; init; }
    public string? Nick { get; set; }
    public bool GamificationAllowed { get; set; }

    public UserAccount Account { get; init; } = new UserAccount();
    public ICollection<SaleTransaction> OpenTransactions { get; init; } = [];
    public ICollection<Transaction> StartedTransactions { get; init; } = [];
    public ICollection<Transaction> CancelledTransactions { get; init; } = [];
    public ICollection<ContainerChange> ContainerChanges { get; init; } = [];
    public ICollection<DiscountUsage> DiscountUsages { get; init; } = [];
    public ICollection<Cost> Costs { get; init; } = [];
}
