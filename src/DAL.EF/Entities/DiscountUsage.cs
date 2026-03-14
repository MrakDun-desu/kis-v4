namespace KisV4.DAL.EF.Entities;

public record DiscountUsage {
    public int Id { get; init; }
    public DateTimeOffset Timestamp { get; init; }

    public int DiscountId { get; init; }
    public Discount? Discount { get; set; }
    public string UserId { get; init; } = string.Empty;
    public User? User { get; set; }
    public ICollection<PriceChange> PriceChanges { get; init; } = [];
}
