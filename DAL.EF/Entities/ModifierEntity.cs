namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents a sale item modifier, such as a vegan toast.
/// </summary>
public record ModifierEntity : SaleItemEntity {
    public required int ModificationTargetId { get; set; }
    public virtual SaleItemEntity? ModificationTarget { get; set; }

    /// <summary>
    /// Sale transaction items this modifier has been applied on.
    /// </summary>
    public virtual ICollection<SaleTransactionItemEntity> Applications { get; private set; } =
        new List<SaleTransactionItemEntity>();
}
