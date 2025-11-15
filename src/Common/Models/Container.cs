using KisV4.Common.Enums;
using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record ContainerListModel {
    public required int Id { get; init; }
    public required decimal Amount { get; init; }
    public required ContainerState State { get; init; }
    public required ContainerTemplateModel Template { get; init; }
    public required PipeListModel Pipe { get; init; }
    public required StoreListModel Store { get; init; }
}

public record ContainerDetailModel {
    public required int Id { get; init; }
    public required decimal Amount { get; init; }
    public required ContainerState State { get; init; }
    public required ContainerTemplateModel Template { get; init; }
    public required PipeListModel Pipe { get; init; }
    public required StoreListModel Store { get; init; }
    public required ContainerChangeReadAllResponse ContainerChanges { get; init; }
}

// Requests and responses
public record ContainerReadAllRequest : PagedRequest {
    public int? StoreId { get; init; }
    public int? TemplateId { get; init; }
    public int? PipeId { get; init; }
    public bool IncludeUnusable { get; init; }
}

public record ContainerReadAllResponse : PagedResponse<ContainerListModel>;

public record ContainerCreateRequest {
    public required int Id { get; init; }
    public required int TemplateId { get; init; }
    public required int StoreId { get; init; }
    public required int Amount { get; init; }
    public required decimal Cost { get; init; }
}

public record ContainerCreateResponse : CollectionResponse<ContainerListModel>;

public record ContainerUpdateRequest {
    public required int StoreId { get; init; }
    public int? PipeId { get; init; }
}

public record ContainerUpdateResponse : ContainerListModel;

public record ContainerReadResponse : ContainerDetailModel;
