namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a product. Product can be a sale item or a store item.
/// </summary>
public record ProductEntity {
    public required int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public bool Deleted { get; set; }
    public ICollection<ProductCategoryEntity> Categories { get; init; } = new List<ProductCategoryEntity>();
    public ICollection<CurrencyCostEntity> Costs { get; init; } = new List<CurrencyCostEntity>();
}
