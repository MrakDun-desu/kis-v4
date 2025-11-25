using System.ComponentModel.DataAnnotations.Schema;

namespace KisV4.DAL.EF.Entities;

public record User {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required int Id { get; init; }

    public Account PrestigeAccount { get; init; } = new();

    public ICollection<SaleTransaction> OpenTransactions { get; init; } = [];
    public ICollection<Transaction> StartedTransactions { get; init; } = [];
    public ICollection<Transaction> CancelledTransactions { get; init; } = [];
    public ICollection<StockTaking> StockTakings { get; init; } = [];
    public ICollection<ContainerChange> ContainerChanges { get; init; } = [];
    public ICollection<DiscountUsage> DiscountUsages { get; init; } = [];
}
