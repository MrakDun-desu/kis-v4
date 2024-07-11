namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a category of products.
/// </summary>
public record ProductCategoryEntity {
    public required int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Products that are in this category.
    /// </summary>
    public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
