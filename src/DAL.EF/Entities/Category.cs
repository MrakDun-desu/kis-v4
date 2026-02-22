namespace KisV4.DAL.EF.Entities;

public record Category {
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;

    public ICollection<StoreItem> StoreItems { get; init; } = [];
    public ICollection<Composite> Composites { get; init; } = [];
}
