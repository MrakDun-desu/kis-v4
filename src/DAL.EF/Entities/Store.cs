namespace KisV4.DAL.EF.Entities;

public record Store {
    public int Id { get; init; }
    public required string Name { get; set; }
    public bool Deleted { get; set; }

    public ICollection<StoreTransactionItem> StoreTransactionItems { get; init; } = [];
    public ICollection<Container> Containers { get; init; } = [];
}
