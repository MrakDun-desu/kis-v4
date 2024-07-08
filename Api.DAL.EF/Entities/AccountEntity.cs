namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents any account, can be a cashbox or user account.
/// </summary>
public record AccountEntity {
    public required int Id { get; init; }
}
