namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a sale item. Sale item can be composed of multiple store items with given amounts.
/// </summary>
public record SaleItemEntity : ProductEntity {
    /// <summary>
    /// Whether to show the sale item on the Kachna Online website.
    /// </summary>
    public bool ShowOnWeb { get; set; }

    /// <summary>
    /// Amounts of store items that this sale item is composed of.
    /// </summary>
    public virtual ICollection<CompositionEntity> Composition { get; private set; } =
        new List<CompositionEntity>();

    /// <summary>
    /// Sale transaction items that this sale item is a part of.
    /// </summary>
    public ICollection<SaleTransactionItemEntity> SaleTransactionItems { get; private set; } =
        new List<SaleTransactionItemEntity>();

    /// <summary>
    /// Available modifiers for this sale item.
    /// </summary>
    public virtual ICollection<ModifierEntity> AvailableModifiers { get; private set; } =
        new List<ModifierEntity>();
}
