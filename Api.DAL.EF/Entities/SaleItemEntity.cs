namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a sale item. Sale item can be composed of multiple store items with given amounts.
/// </summary>
public record SaleItemEntity : ProductEntity {
    public bool ShowOnWeb { get; set; }

    public ICollection<CompositionEntity> Composition { get; set; } =
        new List<CompositionEntity>();
}
