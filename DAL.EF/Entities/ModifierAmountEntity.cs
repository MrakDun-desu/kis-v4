using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents an application of a modifier on a sale transaction item.
/// </summary>
[PrimaryKey(nameof(ModifierId), nameof(SaleTransactionItemId))]
public record ModifierAmountEntity
{
    public int ModifierId { get; set; }

    /// <summary>
    ///     Modifier that has been applied on this sale transaction item.
    /// </summary>
    public virtual ModifierEntity? Modifier { get; set; }

    public int SaleTransactionItemId { get; set; }

    /// <summary>
    ///     Sale transaction that this modifier has been applied on.
    /// </summary>
    public virtual SaleTransactionItemEntity? SaleTransactionItem { get; set; }

    /// <summary>
    ///     Amount of applications of given modifier on given sale transaction item.
    /// </summary>
    public int Amount { get; set; }
}