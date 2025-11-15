using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record ContainerTemplateModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required decimal Amount { get; init; }
    public required StoreItemListModel StoreItem { get; init; }
}

// Requests and responses
public record ContainerTemplateReadAllRequest : PagedRequest {
    public string? Name { get; init; }
    public int? StoreItemId { get; init; }
}

public record ContainerTemplateReadAllResponse : PagedResponse<ContainerTemplateModel>;

public record ContainerTemplateCreateRequest {
    public required string Name { get; init; }
    public required decimal Amount { get; init; }
    public required int StoreItemId { get; init; }
}

public record ContainerTemplateCreateResponse : ContainerTemplateModel;

public record ContainerTemplateUpdateRequest {
    public required string Name { get; init; }
    public required decimal Amount { get; init; }
    public required int StoreItemId { get; init; }
}

public record ContainerTemplateUpdateResponse : ContainerTemplateModel;
