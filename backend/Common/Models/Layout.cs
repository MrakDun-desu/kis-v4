using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record LayoutListModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Image { get; init; }
    public required bool TopLevel { get; set; }
}

public record LayoutDetailModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Image { get; init; }
    public required bool TopLevel { get; init; }
    public required LayoutItemReadAllResponse LayoutItems { get; init; }
}

// Requests and responses
public record LayoutReadAllRequest {
    public bool? TopLevel { get; init; }
    public string? Name { get; init; }
}

public record LayoutReadAllResponse : CollectionResponse<LayoutListModel>;

public record LayoutCreateRequest {
    public required string Name { get; init; }
    public string? Image { get; init; }
    public required bool TopLevel { get; init; }
    public required LayoutItemCreateRequest[] LayoutItems { get; init; }
}

public record LayoutCreateResponse : LayoutDetailModel;

public record LayoutUpdateRequest {
    public required string Name { get; init; }
    public string? Image { get; init; }
    public required bool TopLevel { get; init; }
    public required LayoutItemCreateRequest[] LayoutItems { get; init; }
}

public record LayoutUpdateResponse : LayoutDetailModel;

public record LayoutReadResponse : LayoutDetailModel;
