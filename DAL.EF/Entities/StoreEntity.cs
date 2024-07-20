namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a store.
/// </summary>
public record StoreEntity {
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public bool Deleted { get; set; }

    /// <summary>
    /// Store transaction items that have been applied to this store.
    /// </summary>
    public ICollection<StoreTransactionItemEntity> StoreTransactionItems { get; private set; }
        = new List<StoreTransactionItemEntity>();
}
