using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[PrimaryKey(nameof(StoreItemId), nameof(CompositeId))]
public record Composition {
    public decimal Amount { get; set; }

    public int StoreItemId { get; init; }
    public StoreItem? StoreItem { get; set; }
    public int CompositeId { get; init; }
    public Composite? Composite { get; set; }
}
