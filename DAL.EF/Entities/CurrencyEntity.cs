namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a type of currency, like CZK, EUR or others.
/// </summary>
public record CurrencyEntity {
    public required int Id { get; init; }
    public string Name { get; set; } = string.Empty;
}
