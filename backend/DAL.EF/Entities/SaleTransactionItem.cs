namespace KisV4.DAL.EF.Entities;

public record SaleTransactionItem {
    public int Id { get; init; }
    public int ItemAmount { get; set; }
    public bool Cancelled { get; set; }

    public int SaleTransactionId { get; init; }
    public SaleTransaction? SaleTransaction { get; set; }
    public int SaleItemId { get; init; }
    public SaleItem? SaleItem { get; set; }
    public ICollection<Modification> Modifications { get; init; } = [];
    public ICollection<PriceChange> PriceChanges { get; init; } = [];
}
