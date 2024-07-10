namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a sale item. Sale item can be composed of multiple store items with given amounts.
/// </summary>
public record SaleItemEntity : ProductEntity {
    /// <summary>
    /// Whether to show the sale item on the Kachna Online website.
    /// </summary>
    public bool ShowOnWeb { get; set; }

    public ICollection<CompositionEntity> Composition { get; set; } =
        new List<CompositionEntity>();
}
