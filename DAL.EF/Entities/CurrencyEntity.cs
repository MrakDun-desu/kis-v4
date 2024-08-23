namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a type of currency, like CZK, EUR or others.
/// </summary>
public record CurrencyEntity {
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
}
