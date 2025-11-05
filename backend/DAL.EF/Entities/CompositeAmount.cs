using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[PrimaryKey(nameof(StoreId), nameof(CompositeId))]
public record CompositeAmount {
    public decimal Amount { get; set; }

    public int CompositeId { get; init; }
    public Composite? Composite { get; set; }
    public int StoreId { get; init; }
    public Store? Store { get; set; }
}
