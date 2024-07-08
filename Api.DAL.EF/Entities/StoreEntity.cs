namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a store.
/// </summary>
public record StoreEntity {
    public required int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Deleted { get; set; }
}
