using System.ComponentModel;
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
    [DefaultValue("Kofola 50l")]
    public string Name { get; init; } = string.Empty;
    [DefaultValue(typeof(decimal), "50")]
    public decimal Amount { get; init; }
    public int StoreItemId { get; init; }
}

public record ContainerTemplateCreateResponse : ContainerTemplateModel;

public record ContainerTemplateUpdateRequest {
    public int Id { get; init; }
    [DefaultValue("Kofola 50l")]
    public string Name { get; init; } = string.Empty;
    [DefaultValue(typeof(decimal), "50")]
    public decimal Amount { get; init; }
    public int StoreItemId { get; init; }
}

public record ContainerTemplateUpdateCommand {
    public required ContainerTemplateUpdateRequest Request { get; init; }
    public required int Id { get; init; }
}

public record ContainerTemplateUpdateResponse : ContainerTemplateModel;
