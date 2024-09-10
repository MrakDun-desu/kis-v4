namespace KisV4.Common.Models;

public record StoreAmountSaleItemListModel(
    StoreListModel Store,
    int SaleItemId,
    int Amount
);

public record StoreAmountStoreItemListModel(
    StoreListModel Store,
    int StoreItemId,
    decimal Amount
);

public record StoreItemAmountListModel(
    int StoreId,
    StoreItemListModel StoreItem,
    decimal Amount
);