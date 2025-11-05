using Audit.EntityFramework;

namespace KisV4.DAL.EF.Entities;

public record Category {
    public int Id { get; init; }
    public required string Name { get; set; }

    public ICollection<StoreItem> StoreItems { get; init; } = [];
    public ICollection<Composite> Composites { get; init; } = [];
}
