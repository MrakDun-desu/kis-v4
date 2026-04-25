namespace KisV4.DAL.EF.Entities;

public record SaleItem : Composite {
    public bool SendToFood { get; set; }
    public bool TriggerTablePicker { get; set; }

    public ICollection<Modifier> ApplicableModifiers { get; init; } = [];
    public ICollection<SaleTransactionItem> SaleTransactionItems { get; init; } = [];
}
