using System.ComponentModel;
using KisV4.Common.ModelWrappers;
using Microsoft.AspNetCore.Mvc;

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
    [DefaultValue("Kofola 50l")]
    public required string Name { get; init; }
    [DefaultValue(typeof(decimal), "50")]
    public required decimal Amount { get; init; }
    public required int StoreItemId { get; init; }
}

public record ContainerTemplateCreateResponse : ContainerTemplateModel;

public record ContainerTemplateUpdateRequestModel {
    [DefaultValue("Kofola 50l")]
    public required string Name { get; init; }
    [DefaultValue(typeof(decimal), "50")]
    public required decimal Amount { get; init; }
    public required int StoreItemId { get; init; }
}

public record ContainerTemplateUpdateRequest {
    [FromRoute]
    public required int Id { get; init; }
    [FromBody]
    public required ContainerTemplateUpdateRequestModel Model { get; init; }
}

public record ContainerTemplateUpdateResponse : ContainerTemplateModel;
