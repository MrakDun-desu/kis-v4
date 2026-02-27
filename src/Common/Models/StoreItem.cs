using System.ComponentModel;
using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record StoreItemListModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string UnitName { get; init; }
    public required bool IsContainerItem { get; init; }
    public required decimal CurrentCost { get; init; }
}

public record StoreItemDetailModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string UnitName { get; init; }
    public required bool IsContainerItem { get; init; }
    public required decimal CurrentCost { get; init; }
    public required IEnumerable<CategoryModel> Categories { get; init; }
    public required IEnumerable<CostModel> Costs { get; init; }
}

// Requests and responses
public record StoreItemReadAllRequest : PagedRequest {
    public string? Name { get; init; }
    public bool? IsContainerItem { get; init; }
    public int? CategoryId { get; init; }
}

public record StoreItemReadAllResponse : PagedResponse<StoreItemListModel>;

public record StoreItemCreateRequest {
    [DefaultValue("Kofola")]
    public string Name { get; init; } = string.Empty;
    [DefaultValue("l")]
    public string UnitName { get; init; } = string.Empty;
    [DefaultValue(false)]
    public bool IsContainerItem { get; init; }
    [DefaultValue(new int[0])]
    public int[] CategoryIds { get; init; } = [];
    [DefaultValue(typeof(decimal), "20")]
    public decimal InitialCost { get; init; }
}

public record StoreItemCreateResponse : StoreItemDetailModel;

public record StoreItemUpdateRequest {
    [DefaultValue("Kofola")]
    public string Name { get; init; } = string.Empty;
    [DefaultValue("l")]
    public string UnitName { get; init; } = string.Empty;
    [DefaultValue(new int[0])]
    public int[] CategoryIds { get; init; } = [];
}

public record StoreItemUpdateResponse : StoreItemDetailModel;

public record StoreItemReadResponse : StoreItemDetailModel;
