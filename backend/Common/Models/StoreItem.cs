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
    public required CategoryReadAllResponse Categories { get; init; }
    public required CostReadAllResponse Costs { get; init; }
}

// Requests and responses
public record StoreItemReadAllRequest : PagedRequest {
    public string? Name { get; init; }
    public bool? IsContainerItem { get; init; }
    public int? CategoryId { get; init; }
}

public record StoreItemReadAllResponse : PagedResponse<StoreItemListModel>;

public record StoreItemCreateRequest {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string UnitName { get; init; }
    public required bool IsContainerItem { get; init; }
    public required int[] CategoryIds { get; init; }
}

public record StoreItemCreateResponse : StoreItemDetailModel;

public record StoreItemUpdateRequest {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string UnitName { get; init; }
    public required bool IsContainerItem { get; init; }
    public required int[] CategoryIds { get; init; }
}


public record StoreItemUpdateResponse : StoreItemDetailModel;

public record StoreItemReadResponse : StoreItemDetailModel;
