namespace KisV4.DAL.EF.Entities;

public record Tap {
    public int Id { get; init; }
    public required string Name { get; set; }

    public int? ContainerId { get; set; }
    public Container? Container { get; set; }
    public int StoreId { get; set; }
    public Store? Store { get; set; }
}
