using System.ComponentModel;
using KisV4.Common.ModelWrappers;
using Microsoft.AspNetCore.Mvc;

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
    public string? Name { get; init; }
}

public record LayoutReadAllResponse : CollectionResponse<LayoutListModel>;

public record LayoutCreateRequestModel {
    [DefaultValue("Výchozí layout")]
    public required string Name { get; init; } = string.Empty;
    public string? Image { get; init; }
    [DefaultValue(true)]
    public bool TopLevel { get; init; }
    public required LayoutItemCreateRequest[] LayoutItems { get; init; } = [];
}

public record LayoutCreateRequest {
    [FromQuery]
    public int? StoreId { get; init; }
    [FromBody]
    public required LayoutCreateRequestModel Model { get; init; }
};

public record LayoutCreateResponse : LayoutDetailModel;

public record LayoutUpdateRequestModel {
    [DefaultValue("Test")]
    public required string Name { get; init; } = string.Empty;
    public string? Image { get; init; }
    [DefaultValue(false)]
    public bool TopLevel { get; init; }
    public required LayoutItemCreateRequest[] LayoutItems { get; init; } = [];
}

public record LayoutUpdateRequest {
    [FromRoute]
    public required int Id { get; init; }
    [FromBody]
    public required LayoutUpdateRequestModel Model { get; init; }
    [FromQuery]
    public int? StoreId { get; init; }
};

public record LayoutUpdateResponse : LayoutDetailModel;

public record LayoutReadRequest {
    [FromRoute]
    public required int Id { get; init; }
    [FromQuery]
    public int? StoreId { get; init; }
}

public record LayoutReadTopLevelRequest {
    public int? StoreId { get; init; }
}

public record LayoutReadResponse : LayoutDetailModel;
