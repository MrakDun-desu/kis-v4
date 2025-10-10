namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents a type of currency, like CZK, EUR or others.
/// </summary>
public record CurrencyEntity {
    public int Id { get; set; }

    /// <summary>
    ///     Long name of the given currency - for example "czech crowns".
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Short name of the given currency - for example "CZK".
    /// </summary>
    public string ShortName { get; set; } = string.Empty;
}
