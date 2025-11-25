using KisV4.Common.Enums;
using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record ContainerChangeModel {
    public required ContainerState NewState { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
    public required int ContainerId { get; init; }
    public required UserListModel User { get; init; }
}

// Requests and responses
public record ContainerChangeReadAllRequest {
    public required int ContainerId { get; init; }
}

public record ContainerChangeReadAllResponse : CollectionResponse<ContainerChangeModel>;

public record ContainerChangeCreateRequest {
    public required decimal NewAmount { get; init; }
    public required ContainerState NewState { get; init; }
    public required int ContainerId { get; init; }
}

public record ContainerChangeCreateResponse : ContainerChangeModel;
