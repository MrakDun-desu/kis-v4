using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record CostModel {
    public required decimal Amount { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
    public required string Description { get; init; }
    public required StoreItemListModel StoreItem { get; init; }
    public required UserModel User { get; init; }
}

// Requests and responses
public record CostReadAllRequest {
    public required int StoreItemId { get; init; }
}

public record CostReadAllResponse : CollectionResponse<CostModel>;

public record CostCreateRequest {
    public required int StoreItemId { get; init; }
    public required decimal Amount { get; init; }
    public required string Description { get; init; }
}

public record CostCreateResponse : CostModel;
