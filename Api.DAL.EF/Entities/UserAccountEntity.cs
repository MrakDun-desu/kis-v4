namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a user account.
/// </summary>
public record UserAccountEntity : AccountEntity {
    public string Name { get; set; } = string.Empty;
    public bool Deleted { get; set; }
}
