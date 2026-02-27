using System.ComponentModel;
using KisV4.Common.ModelWrappers;

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

// Requests and responses
public record PipeReadAllResponse : CollectionResponse<PipeListModel>;

public record PipeCreateRequest {
    [DefaultValue("Pípa Kachna")]
    public string Name { get; init; } = string.Empty;
}

public record PipeCreateResponse : PipeListModel;

public record PipeUpdateRequest {
    [DefaultValue("Pípa Kachna")]
    public string Name { get; init; } = string.Empty;
}

public record PipeUpdateResponse : PipeListModel;

public record PipeReadResponse : PipeDetailModel;
