namespace KisV4.Common.Models;

public record CompositionCreateModel(
    int SaleItemId,
    int StoreItemId,
    decimal Amount
);

public record CompositionListModel(
    int SaleItemId,
    StoreItemListModel StoreItem,
    decimal Amount
);