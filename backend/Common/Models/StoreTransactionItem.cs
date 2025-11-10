using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record StoreTransactionItemModel {
    public required decimal ItemAmount { get; init; }
    public required decimal Cost { get; init; }
    public required StoreItemListModel StoreItem { get; init; }
    public required StoreListModel Store { get; init; }
    public required int StoreTransactionId { get; init; }
}

// Requests and responses
public record StoreTransactionItemCreateRequest {
    public required decimal ItemAmount { get; init; }
    public required decimal Cost { get; init; }
    public required int StoreItemId { get; init; }
}

public record StoreTransactionItemReadAllResponse : CollectionResponse<StoreTransactionItemModel>;
