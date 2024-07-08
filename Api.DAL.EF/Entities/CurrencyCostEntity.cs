using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a currency cost of a given product.
/// </summary>
[PrimaryKey(nameof(ProductId), nameof(CurrencyId))]
public record CurrencyCostEntity {
    public required int ProductId { get; set; }
    public ProductEntity? Product { get; set; }
    public required int CurrencyId { get; set; }
    public CurrencyEntity? Currency { get; set; }

    [Precision(11,2)]
    public decimal Amount { get; set; }
    public DateTime ValidSince { get; init; }
    public string Description { get; set; } = string.Empty;
    public bool IsExclusive { get; set; } // whether the cost is exclusive with other costs
}
