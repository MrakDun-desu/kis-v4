using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a sale transaction item.
/// </summary>
[PrimaryKey(nameof(SaleTransactionId), nameof(SaleItemId))]
public record SaleTransactionItemEntity {
    public required int SaleItemId { get; init; }
    public SaleItemEntity? SaleItem { get; set; }
    public required int SaleTransactionId { get; init; }
    public SaleTransactionEntity? SaleTransaction { get; set; }

    public ICollection<ModifierEntity> Modifiers { get; set; } = new List<ModifierEntity>();
    /// <summary>
    /// Prices paid for this transaction item with various currencies.
    /// </summary>
    public ICollection<TransactionPriceEntity> TransactionPrices { get; set; } =
        new List<TransactionPriceEntity>();
    /// <summary>
    /// Amount of a sale item that has been sold in this transaction.
    /// </summary>
    public int ItemAmount { get; set; }
}
