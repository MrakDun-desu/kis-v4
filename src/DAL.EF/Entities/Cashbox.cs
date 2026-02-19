namespace KisV4.DAL.EF.Entities;

public record Cashbox {
    public int Id { get; init; }
    public bool Deleted { get; set; }
    public required string Name { get; set; }

    public int SalesAccountId { get; init; }
    public Account? SalesAccount { get; init; } = new();
    public int DonationsAccountId { get; init; }
    public Account? DonationsAccount { get; init; } = new();
}
