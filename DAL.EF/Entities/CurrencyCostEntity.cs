using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a currency cost of a given product.
/// </summary>
public record CurrencyCostEntity {
    public required int Id { get; init; }
    public required int ProductId { get; init; }
    /// <summary>
    /// Product that has this cost.
    /// </summary>
    public virtual ProductEntity? Product { get; set; }
    public required int CurrencyId { get; init; }
    /// <summary>
    /// Currency that is part of the cost of given product.
    /// </summary>
    public virtual CurrencyEntity? Currency { get; set; }
    public DateTime ValidSince { get; init; }

    [Precision(11,2)]
    public decimal Amount { get; set; }
    /// <summary>
    /// Description of this cost or a reason why this cost is given to a product.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
