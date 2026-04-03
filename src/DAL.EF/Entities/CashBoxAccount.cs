namespace KisV4.DAL.EF.Entities;

public record CashBoxAccount : Account {
    public int CashboxId { get; init; }
    public Cashbox? Cashbox { get; init; }
}
