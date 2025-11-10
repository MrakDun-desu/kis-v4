using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record ModifierListModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Image { get; init; }
    public decimal MarginPercent { get; init; }
    public decimal MarginStatic { get; init; }
    public decimal PrestigeAmount { get; init; }
}

public record ModifierDetailModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Image { get; init; }
    public decimal MarginPercent { get; init; }
    public decimal MarginStatic { get; init; }
    public decimal PrestigeAmount { get; init; }
    public required CategoryReadAllResponse Categories { get; init; }
    public required CompositionReadAllResponse Compositions { get; init; }
    public required SaleItemReadAllResponse Targets { get; init; }
}

// Requests and responses
public record ModifierReadAllRequest : PagedRequest {
    public string? Name { get; init; }
    public int? CategoryId { get; init; }
    public int? TargetId { get; init; }
}

public record ModifierReadAllResponse : PagedResponse<ModifierListModel>;

public record ModifierCreateRequest {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Image { get; init; }
    public required decimal MarginPercent { get; init; }
    public required decimal MarginStatic { get; init; }
    public required decimal PrestigeAmount { get; init; }
    public required int[] CategoryIds { get; init; }
}

public record ModifierCreateResponse : ModifierDetailModel;

public record ModifierUpdateRequest {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Image { get; init; }
    public required decimal MarginPercent { get; init; }
    public required decimal MarginStatic { get; init; }
    public required decimal PrestigeAmount { get; init; }
    public required int[] CategoryIds { get; init; }
}

public record ModifierUpdateResponse : ModifierDetailModel;

public record ModifierReadResponse : ModifierDetailModel;
