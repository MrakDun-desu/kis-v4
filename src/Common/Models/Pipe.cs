using System.ComponentModel;
using KisV4.Common.ModelWrappers;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.Common.Models;

// Base models
// Used in admin for list view
public record PipeListModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
}

// Used in operator for displaying containers available in a pipe
public record PipeDetailModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required IEnumerable<ContainerPipeModel> Containers { get; init; }
}

public record PipeUpdateModel {
    [DefaultValue("Pípa Kachna")]
    public required string Name { get; init; }
}

// Requests and responses
public record PipeReadAllResponse : CollectionResponse<PipeListModel>;

public record PipeCreateRequest {
    [DefaultValue("Pípa Kachna")]
    public required string Name { get; init; }
}

public record PipeCreateResponse : PipeListModel;

public record PipeUpdateRequest {
    [FromRoute]
    public required int Id { get; init; }
    [FromBody]
    public required PipeUpdateModel Model { get; init; }
}

public record PipeUpdateResponse : PipeListModel;

public record PipeReadResponse : PipeDetailModel;
