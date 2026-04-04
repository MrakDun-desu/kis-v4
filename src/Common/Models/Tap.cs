using System.ComponentModel;
using KisV4.Common.ModelWrappers;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.Common.Models;

// Base models
// Used in admin for list view
public record TapListModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int? ContainerId { get; init; }
    public required StoreListModel Store { get; init; }
}

public record TapDetailModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int? ContainerId { get; init; }
    public required StoreListModel Store { get; init; }
    public required IEnumerable<ContainerListModel> Containers { get; init; }
}

public record TapUpdateModel {
    [DefaultValue("Kachna 1")]
    public required string Name { get; init; }
    public int? ContainerId { get; init; }
}

// Requests and responses
public record TapReadAllResponse : CollectionResponse<TapListModel>;

public record TapCreateRequest {
    [DefaultValue("Kachna 1")]
    public required string Name { get; init; }
    public required int StoreId { get; init; }
}

public record TapCreateResponse : TapListModel;

public record TapUpdateRequest {
    [FromRoute]
    public required int Id { get; init; }
    [FromBody]
    public required TapUpdateModel Model { get; init; }
}

public record TapUpdateResponse : TapListModel;

public record TapReadRequest {
    [FromRoute]
    public required int Id { get; init; }
}

public record TapReadResponse : TapDetailModel;
