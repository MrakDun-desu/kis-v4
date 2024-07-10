namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a sale item modifier, such as a vegan toast.
/// </summary>
public record ModifierEntity : SaleItemEntity {
    public required int ModificationTargetId { get; set; }
    public SaleItemEntity? ModificationTarget { get; set; }

    public ICollection<SaleTransactionItemEntity> Applications { get; set; } =
        new List<SaleTransactionItemEntity>();
}
