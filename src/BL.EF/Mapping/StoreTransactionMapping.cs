using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class StoreTransactionMapping {
    public static StoreTransactionListModel ToModel(this StoreTransaction source) =>
        new() {
            Id = source.Id,
            Note = source.Note,
            StartedAt = source.StartedAt,
            CancelledAt = source.CancelledAt,
            StartedBy = source.StartedBy.ToModel()!,
            CancelledBy = source.CancelledBy.ToModel(),
            Reason = source.Reason,
            SaleTransactionId = source.SaleTransactionId
        };
}
