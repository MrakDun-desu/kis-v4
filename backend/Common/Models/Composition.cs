using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record CompositionModel {
    public required decimal Amount { get; init; }
    public required StoreItemListModel StoreItem { get; init; }
    public required int CompositeId { get; init; }
}

// Requests and responses
public record CompositionReadAllRequest {
    public required int CompositeId { get; init; }
}

public record CompositionReadAllResponse : CollectionResponse<CompositionModel>;

public record CompositionPutRequest {
    public required int CompositeId { get; init; }
    public required int StoreItemId { get; init; }
    public required decimal Amount { get; init; }
}
