namespace KisV4.Common.Models;

public record CompositionCreateModel(
    int SaleItemId,
    int StoreItemId,
    decimal Amount
);

public record CompositionReadAllModel(
    int SaleItemId,
    int StoreItemId,
    StoreItemReadAllModel StoreItem,
    decimal Amount
);