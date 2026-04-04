namespace KisV4.DAL.EF.Entities;

public record Cashbox {
    public int Id { get; init; }
    public bool Deleted { get; set; }
    public required string Name { get; set; }

    public CashBoxAccount Account { get; init; } = new CashBoxAccount();
}
