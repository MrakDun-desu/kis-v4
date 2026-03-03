using System.ComponentModel;
using KisV4.Common.Enums;
using KisV4.Common.ModelWrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.Common.Models;

// Base models
// Used in admin for list view and in operator for viewing all available pipes
public record ContainerListModel {
    public required int Id { get; init; }
    public required decimal Amount { get; init; }
    public required ContainerState State { get; init; }
    public required ContainerTemplateModel Template { get; init; }
    public required PipeListModel? Pipe { get; init; }
    public required StoreListModel Store { get; init; }
}

// Used in operator when viewing containers at a pipe
public record ContainerPipeModel {
    public required int Id { get; init; }
    public required decimal Amount { get; init; }
    public required ContainerState State { get; init; }
    public required ContainerTemplateModel Template { get; init; }
}

// Used in admin for displaying container detail
public record ContainerDetailModel {
    public required int Id { get; init; }
    public required decimal Amount { get; init; }
    public required ContainerState State { get; init; }
    public required ContainerTemplateModel Template { get; init; }
    public required PipeListModel? Pipe { get; init; }
    public required StoreListModel Store { get; init; }
    public required IEnumerable<ContainerChangeModel> ContainerChanges { get; init; }
}

public record ContainerUpdateModel {
    public required int StoreId { get; init; }
    public int? PipeId { get; init; }
};

// Requests and responses
public record ContainerReadAllRequest : PagedRequest {
    public int? StoreId { get; init; }
    public int? TemplateId { get; init; }
    public int? PipeId { get; init; }
    public bool? IncludeUnusable { get; init; }
}

public record ContainerReadAllResponse : PagedResponse<ContainerListModel>;

public record ContainerCreateRequest {
    public required int TemplateId { get; init; }
    public required int StoreId { get; init; }
    public required int Amount { get; init; }
    [DefaultValue(typeof(decimal), "50")]
    public required decimal Cost { get; init; }
    [DefaultValue(false)]
    public bool UpdateCosts { get; init; }
}

public record ContainerCreateResponse : CollectionResponse<ContainerListModel>;

public record ContainerUpdateRequest {
    [FromRoute]
    public required int Id { get; init; }
    [FromBody]
    public required ContainerUpdateModel Model { get; init; }
};

public record ContainerUpdateResponse : ContainerListModel;

public record ContainerReadResponse : ContainerDetailModel;

public record ContainerOperatorReadResponse {
    public required int Id { get; init; }
    public required decimal Amount { get; init; }
    public required ContainerState State { get; init; }
    public required ContainerTemplateModel Template { get; init; }
    public required IEnumerable<SaleItemOperatorModel> SaleItems { get; init; }
}
