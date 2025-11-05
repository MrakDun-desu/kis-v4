namespace KisV4.DAL.EF.Entities;

public record DiscountUsage {
    public int Id { get; init; }
    public DateTimeOffset Timestamp { get; init; }

    public int DiscountId { get; init; }
    public Discount? Discount { get; set; }
    public int UserId { get; init; }
    public User? User { get; set; }
    public ICollection<PriceChange> PriceChanges { get; init; } = [];
}
