using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[PrimaryKey(nameof(StoreId), nameof(StoreItemId))]
public record StoreItemAmount {
    public decimal Amount { get; set; }

    public int StoreItemId { get; init; }
    public StoreItem? StoreItem { get; set; }
    public int StoreId { get; init; }
    public Store? Store { get; set; }
}
