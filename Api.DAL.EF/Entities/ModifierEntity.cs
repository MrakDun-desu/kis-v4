namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a sale item modifier, such as a vegan toast.
/// </summary>
public record ModifierEntity : SaleItemEntity {
    public required int ModificationTargetId { get; set; }
    public SaleItemEntity? ModificationTarget { get; set; }

    /// <summary>
    /// Sale transaction items this modifier has been applied on.
    /// </summary>
    public ICollection<SaleTransactionItemEntity> Applications { get; set; } =
        new List<SaleTransactionItemEntity>();
}
