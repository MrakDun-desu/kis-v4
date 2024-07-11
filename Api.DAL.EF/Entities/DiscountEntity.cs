namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a discount that certain users can use to alter prices of products in a sale transaction.
/// </summary>
public record DiscountEntity {
    public required int Id { get; init; }
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Each single time this discount was used.
    /// Can have multiple items and currencies to be applied to.
    /// </summary>
    public ICollection<DiscountUsageEntity> DiscountUsages { get; init; } =
        new List<DiscountUsageEntity>();
}
