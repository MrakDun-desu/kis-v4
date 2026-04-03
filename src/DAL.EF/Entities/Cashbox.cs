namespace KisV4.DAL.EF.Entities;

public record Cashbox {
    public int Id { get; init; }
    public bool Deleted { get; set; }
    public required string Name { get; set; }

    public required ICollection<CashBoxAccount> Accounts { get; init; }
}
