namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a product. Product can be a sale item or a store item.
/// </summary>
public abstract record ProductEntity {
    public required int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public bool Deleted { get; set; }
    /// <summary>
    /// Categories that this product is in.
    /// </summary>
    public virtual ICollection<ProductCategoryEntity> Categories { get; private set; } = new List<ProductCategoryEntity>();
    /// <summary>
    /// Costs that this product has.
    /// </summary>
    public virtual ICollection<CurrencyCostEntity> Costs { get; private set; } = new List<CurrencyCostEntity>();
}
