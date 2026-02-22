using KisV4.Common.Enums;
using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record SaleItemListModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Image { get; init; }
    public required decimal MarginPercent { get; init; }
    public required decimal MarginStatic { get; init; }
    public required decimal PrestigeAmount { get; init; }
    public required PrintType PrintType { get; init; }
}

public record SaleItemDetailModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Image { get; init; }
    public required decimal MarginPercent { get; init; }
    public required decimal MarginStatic { get; init; }
    public required decimal PrestigeAmount { get; init; }
    public required PrintType PrintType { get; init; }
    public required IEnumerable<ModifierListModel> ApplicableModifiers { get; init; }
}

// Requests and responses
public record SaleItemReadAllRequest : PagedRequest {
    public string? Name { get; init; }
    public int? CategoryId { get; init; }
}

public record SaleItemReadAllResponse : PagedResponse<SaleItemListModel>;

public record SaleItemCreateRequest {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Image { get; init; }
    public required decimal MarginPercent { get; init; }
    public required decimal MarginStatic { get; init; }
    public required decimal PrestigeAmount { get; init; }
    public required PrintType PrintType { get; init; }
    public required int[] CategoryIds { get; init; }
}

public record SaleItemCreateResponse : SaleItemDetailModel;

public record SaleItemUpdateRequest {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Image { get; init; }
    public required decimal MarginPercent { get; init; }
    public required decimal MarginStatic { get; init; }
    public required decimal PrestigeAmount { get; init; }
    public required PrintType PrintType { get; init; }
    public required int[] CategoryIds { get; init; }
}

public record SaleItemUpdateResponse : SaleItemDetailModel;

public record SaleItemReadResponse : SaleItemDetailModel;
