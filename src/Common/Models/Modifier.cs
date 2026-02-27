using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record ModifierListModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Image { get; init; }
    public required decimal MarginPercent { get; init; }
    public required decimal MarginStatic { get; init; }
    public required decimal PrestigeAmount { get; init; }
}

public record ModifierDetailModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Image { get; init; }
    public required decimal MarginPercent { get; init; }
    public required decimal MarginStatic { get; init; }
    public required decimal PrestigeAmount { get; init; }
    public required IEnumerable<CategoryModel> Categories { get; init; }
    public required IEnumerable<SaleItemListModel> Targets { get; init; }
}

// Requests and responses
public record ModifierReadAllRequest : PagedRequest {
    public string? Name { get; init; }
    public int? CategoryId { get; init; }
    public int? TargetId { get; init; }
}

public record ModifierReadAllResponse : PagedResponse<ModifierListModel>;

public record ModifierCreateRequest(
    string Name,
    string? Image,
    decimal MarginPercent,
    decimal MarginStatic,
    decimal PrestigeAmount,
    int[] CategoryIds,
    int[] TargetIds
);

public record ModifierCreateResponse : ModifierDetailModel;

public record ModifierUpdateRequest(
    string Name,
    string? Image,
    decimal MarginPercent,
    decimal MarginStatic,
    decimal PrestigeAmount,
    int[] CategoryIds,
    int[] TargetIds
);

public record ModifierUpdateResponse : ModifierDetailModel;

public record ModifierReadResponse : ModifierDetailModel;
