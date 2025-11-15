namespace KisV4.DAL.EF.Entities;

public record LayoutSaleItem : LayoutItem {
    public int TargetId { get; init; }
    public SaleItem? Target { get; set; }
}
