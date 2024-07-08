namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a category of products.
/// </summary>
public record ProductCategoryEntity {
    public required int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
