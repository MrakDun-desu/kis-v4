using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[PrimaryKey(nameof(StoreItemId), nameof(StoreId), nameof(StoreTransactionId))]
public record StoreTransactionItem {
    public decimal ItemAmount { get; set; }
    public decimal Cost { get; set; }
    public bool Cancelled { get; set; }

    public int StoreItemId { get; init; }
    public StoreItem? StoreItem { get; set; }
    public int StoreId { get; init; }
    public Store? Store { get; set; }
    public int StoreTransactionId { get; init; }
    public StoreTransaction? StoreTransaction { get; set; }
}
