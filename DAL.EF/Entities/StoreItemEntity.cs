namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents a store item.
/// </summary>
public record StoreItemEntity : ProductEntity {
    /// <summary>
    ///     Name of the unit of this store item to be displayed (ks, l, g...)
    /// </summary>
    public string UnitName { get; set; } = string.Empty;

    /// <summary>
    ///     Whether a barman can stock this store item.
    /// </summary>
    public bool BarmanCanStock { get; set; }

    /// <summary>
    ///     If this is set, this item shouldn't be managed through normal stores, only through
    ///     containers that have it set as contained item.
    /// </summary>
    public bool IsContainerItem { get; set; }

    /// <summary>
    ///     Store transaction items that this store item is a part of.
    /// </summary>
    public virtual ICollection<StoreTransactionItemEntity> StoreTransactionItems { get; private set; } =
        [];
}
