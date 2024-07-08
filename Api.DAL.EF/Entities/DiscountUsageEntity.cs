using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a single usage of a discount by a given user.
/// Can be used on multiple items to discount multiple currencies.
/// </summary>
public record DiscountUsageEntity {
    public required int Id { get; init; }
    public required int UserId { get; init; }
    public UserAccountEntity? User { get; set; }
    public required int DiscountId { get; init; }
    public DiscountEntity? Discount { get; set; }

    public DateTime Timestamp { get; set; }

    public ICollection<DiscountUsageItemEntity> UsageItems { get; set; } =
        new List<DiscountUsageItemEntity>();
}
