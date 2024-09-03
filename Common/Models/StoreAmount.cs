namespace KisV4.Common.Models;

public record StoreAmountListModel(
    StoreListModel Store,
    int ProductId,
    int Amount
);

public record StoreItemAmountListModel(
    int StoreId,
    StoreItemListModel StoreItem,
    decimal Amount
);