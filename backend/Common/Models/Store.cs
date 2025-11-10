using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record StoreListModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
}

public record StoreDetailModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required StoreItemAmountReadAllResponse StoreItemAmounts { get; init; }
    public required ContainerReadAllResponse Containers { get; init; }
}

// Requests and responses
public record StoreReadAllRequest : PagedRequest;

public record StoreReadAllResponse : PagedResponse<StoreListModel>;

public record StoreCreateRequest {
    public required string Name { get; init; }
}

public record StoreCreateResponse : StoreListModel;

public record StoreUpdateRequest {
    public required string Name { get; init; }
}

public record StoreUpdateResponse : StoreListModel;

public record StoreReadRequest : StoreDetailModel;
