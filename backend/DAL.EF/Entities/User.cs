using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[Index(nameof(Username), IsUnique = true)]
public record User {
    public int Id { get; init; }
    public required string Username { get; init; }

    public Account PrestigeAccount { get; init; } = new();

    public ICollection<SaleTransaction> OpenTransactions { get; init; } = [];
    public ICollection<Transaction> StartedTransactions { get; init; } = [];
    public ICollection<Transaction> CancelledTransactions { get; init; } = [];
    public ICollection<StockTaking> StockTakings { get; init; } = [];
    public ICollection<ContainerChange> ContainerChanges { get; init; } = [];
    public ICollection<DiscountUsage> DiscountUsages { get; init; } = [];
}
