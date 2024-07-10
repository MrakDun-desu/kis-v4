using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a currency cost of a given product.
/// </summary>
[PrimaryKey(nameof(ProductId), nameof(CurrencyId), nameof(ValidSince))]
public record CurrencyCostEntity {
    public required int ProductId { get; init; }
    public ProductEntity? Product { get; set; }
    public required int CurrencyId { get; init; }
    public CurrencyEntity? Currency { get; set; }
    public DateTime ValidSince { get; init; }

    [Precision(11,2)]
    public decimal Amount { get; set; }
    /// <summary>
    /// Description of this cost or a reason why this cost is given to a product.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
