using System.ComponentModel;
using KisV4.Common.ModelWrappers;
using Microsoft.AspNetCore.Mvc;

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

public record ModifierUpdateModel(
    [DefaultValue("Vegánský toust")]
    string Name,
    string? Image,
    [DefaultValue(typeof(decimal), "5")]
    decimal MarginPercent,
    [DefaultValue(typeof(decimal), "0")]
    decimal MarginStatic,
    [DefaultValue(typeof(decimal), "0")]
    decimal PrestigeAmount,
    [DefaultValue(new int[0])]
    int[] CategoryIds,
    [DefaultValue(new int[0])]
    int[] TargetIds
);

// Requests and responses
public record ModifierReadAllRequest : PagedRequest {
    public string? Name { get; init; }
    public int? CategoryId { get; init; }
    public int? TargetId { get; init; }
}

public record ModifierReadAllResponse : PagedResponse<ModifierListModel>;

public record ModifierCreateRequest(
    [DefaultValue("Vegánský toust")]
    string Name,
    string? Image,
    [DefaultValue(typeof(decimal), "5")]
    decimal MarginPercent,
    [DefaultValue(typeof(decimal), "0")]
    decimal MarginStatic,
    [DefaultValue(typeof(decimal), "0")]
    decimal PrestigeAmount,
    [DefaultValue(new int[0])]
    int[] CategoryIds,
    [DefaultValue(new int[0])]
    int[] TargetIds
);

public record ModifierCreateResponse : ModifierDetailModel;

public record ModifierUpdateRequest(
    [FromRoute]
    int Id,
    [FromBody]
    ModifierUpdateModel Model
);

public record ModifierUpdateResponse : ModifierDetailModel;

public record ModifierReadResponse : ModifierDetailModel;
