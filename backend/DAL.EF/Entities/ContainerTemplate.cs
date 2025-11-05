namespace KisV4.DAL.EF.Entities;

public record ContainerTemplate {
    public int Id { get; init; }
    public required string Name { get; set; }
    public decimal Amount { get; set; }
    public bool Deleted { get; set; }

    public int StoreItemId { get; set; }
    public StoreItem? StoreItem { get; set; }
    public ICollection<Container> Instances { get; init; } = [];
}
