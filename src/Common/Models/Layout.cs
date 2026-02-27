using System.ComponentModel;
using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record LayoutListModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Image { get; init; }
    public required bool TopLevel { get; set; }
}

public record LayoutDetailModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Image { get; init; }
    public required bool TopLevel { get; init; }
    public required IEnumerable<LayoutItemModel> LayoutItems { get; init; }
}

// Requests and responses
public record LayoutReadAllRequest {
    public bool? TopLevel { get; init; }
    public string? Name { get; init; }
}

public record LayoutReadAllResponse : CollectionResponse<LayoutListModel>;

public record LayoutCreateRequest {
    [DefaultValue("Výchozí layout")]
    public string Name { get; init; } = string.Empty;
    public string? Image { get; init; }
    [DefaultValue(true)]
    public bool TopLevel { get; init; }
    public LayoutItemCreateRequest[] LayoutItems { get; init; } = [];
}

public record LayoutCreateResponse : LayoutDetailModel;

public record LayoutUpdateRequest {
    [DefaultValue("Test")]
    public string Name { get; init; } = string.Empty;
    public string? Image { get; init; }
    [DefaultValue(false)]
    public bool TopLevel { get; init; }
    public LayoutItemCreateRequest[] LayoutItems { get; init; } = [];
}

public record LayoutUpdateResponse : LayoutDetailModel;

public record LayoutReadResponse : LayoutDetailModel;
