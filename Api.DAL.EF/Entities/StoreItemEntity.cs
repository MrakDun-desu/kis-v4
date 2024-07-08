namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a store item.
/// </summary>
public record StoreItemEntity : ProductEntity {
    /// <summary>
    /// Name of the unit of this store item to be displayed (ks, l, g...)
    /// </summary>
    public string UnitName { get; set; } = string.Empty;
    /// <summary>
    /// Whether a barman can stock this store item.
    /// </summary>
    public bool BarmanCanStock { get; set; }
    /// <summary>
    /// Whether this item should be managed through containers.
    /// </summary>
    public bool IsContainerItem { get; set; }
}
