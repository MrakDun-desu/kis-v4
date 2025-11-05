namespace KisV4.DAL.EF.Entities;

public record Discount {
    public int Id { get; init; }
    public required string Name { get; set; }
    public bool Deleted { get; set; }

    public ICollection<DiscountUsage> DiscountUsages { get; init; } = [];
}
