using KisV4.Common.Enums;

namespace KisV4.DAL.EF.Entities;

public record SaleItem : Composite {
    public PrintType PrintType { get; set; }

    public ICollection<Modifier> ApplicableModifiers { get; init; } = [];
    public ICollection<SaleTransactionItem> SaleTransactionItems { get; init; } = [];
}
