namespace KisV4.DAL.EF.Entities;

public record UserAccount : Account {
    public string UserId { get; init; } = string.Empty;
    public User? User { get; init; }
}
