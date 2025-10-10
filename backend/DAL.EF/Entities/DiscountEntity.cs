namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents a discount that certain users can use to alter prices of products in a sale transaction.
/// </summary>
public record DiscountEntity {
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public bool Deleted { get; set; }

    /// <summary>
    ///     Each single time this discount was used.
    ///     Can have multiple items and currencies to be applied to.
    /// </summary>
    public virtual ICollection<DiscountUsageEntity> DiscountUsages { get; private set; } =
        [];
}
