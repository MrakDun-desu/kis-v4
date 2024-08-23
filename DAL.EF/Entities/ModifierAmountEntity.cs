using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[PrimaryKey(nameof(ModifierId), nameof(SaleTransactionItemId))]
public record ModifierAmountEntity
{
    public int ModifierId { get; set; }
    public virtual ModifierEntity? Modifier { get; set; }
    public int SaleTransactionItemId { get; set; }
    public virtual SaleTransactionItemEntity? SaleTransactionItem { get; set; }
    public int Amount { get; set; }
}