namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a single usage of a discount by a given user.
/// Can be used on multiple items to discount multiple currencies.
/// </summary>
public record DiscountUsageEntity {
    public required int Id { get; init; }
    public required int UserId { get; init; }
    /// <summary>
    /// User that has used the given discount.
    /// </summary>
    public UserAccountEntity? User { get; set; }
    public required int DiscountId { get; init; }
    /// <summary>
    /// Discount that has been used by the given user.
    /// </summary>
    public DiscountEntity? Discount { get; set; }

    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Each item and currency that this discount usage was applied on.
    /// </summary>
    public ICollection<DiscountUsageItemEntity> UsageItems { get; set; } =
        new List<DiscountUsageItemEntity>();
}
