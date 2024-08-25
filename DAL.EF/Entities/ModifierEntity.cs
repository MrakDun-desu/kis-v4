namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents a sale item modifier, such as a vegan toast.
/// </summary>
public record ModifierEntity : SaleItemEntity
{
    public int ModificationTargetId { get; set; }
    public virtual SaleItemEntity? ModificationTarget { get; set; }
}