namespace KisV4.Common.Models;

public record StoreTransactionItemCreateModel(
    int StoreItemId,
    decimal Amount
);

public record StoreTransactionItemListModel(
    int Id,
    StoreItemListModel StoreItem,
    StoreListModel Store,
    int StoreTransactionId,
    decimal ItemAmount,
    bool Cancelled
);