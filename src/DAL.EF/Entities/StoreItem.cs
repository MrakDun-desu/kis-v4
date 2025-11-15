namespace KisV4.DAL.EF.Entities;

public record StoreItem {
    public int Id { get; init; }
    public required string Name { get; set; }
    public bool Hidden { get; set; }
    public required string UnitName { get; set; }
    public bool IsContainerItem { get; set; }
    public decimal CurrentCost { get; set; }

    public ICollection<Category> Categories { get; init; } = [];
    public ICollection<Composition> Compositions { get; init; } = [];
    public ICollection<StoreTransactionItem> StoreTransactionItems = [];
    public ICollection<Cost> Costs { get; init; } = [];
}
