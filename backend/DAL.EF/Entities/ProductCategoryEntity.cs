namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents a category of products.
/// </summary>
public record ProductCategoryEntity {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Products that are in this category.
    /// </summary>
    public virtual ICollection<ProductEntity> Products { get; private set; } = [];
}
