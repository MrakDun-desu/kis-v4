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
    public string Name { get; init; } = string.Empty;
    [DefaultValue(typeof(decimal), "50")]
    public decimal Amount { get; init; }
    public int StoreItemId { get; init; }
}

public record ContainerTemplateCreateResponse : ContainerTemplateModel;

public record ContainerTemplateUpdateRequestModel {
    [DefaultValue("Kofola 50l")]
    public string Name { get; init; } = string.Empty;
    [DefaultValue(typeof(decimal), "50")]
    public decimal Amount { get; init; }
    public int StoreItemId { get; init; }
}

public record ContainerTemplateUpdateRequest {
    [FromRoute]
    public int Id { get; init; }
    [FromBody]
    public ContainerTemplateUpdateRequestModel Model { get; init; } = default!;
}

public record ContainerTemplateUpdateResponse : ContainerTemplateModel;
