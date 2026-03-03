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

public record ModifierUpdateModel {
    [DefaultValue("Vegánský toust")]
    public required string Name { get; init; }
    public required string? Image { get; init; }
    [DefaultValue(typeof(decimal), "5")]
    public required decimal MarginPercent { get; init; }
    [DefaultValue(typeof(decimal), "0")]
    public required decimal MarginStatic { get; init; }
    [DefaultValue(typeof(decimal), "0")]
    public required decimal PrestigeAmount { get; init; }
    [DefaultValue(new int[0])]
    public required int[] CategoryIds { get; init; }
    [DefaultValue(new int[0])]
    public required int[] TargetIds { get; init; }
};

// Requests and responses
public record ModifierReadAllRequest : PagedRequest {
    public string? Name { get; init; }
    public int? CategoryId { get; init; }
    public int? TargetId { get; init; }
}

public record ModifierReadAllResponse : PagedResponse<ModifierListModel>;

public record ModifierCreateRequest {
    [DefaultValue("Vegánský toust")]
    public required string Name { get; init; }
    public required string? Image { get; init; }
    [DefaultValue(typeof(decimal), "5")]
    public required decimal MarginPercent { get; init; }
    [DefaultValue(typeof(decimal), "0")]
    public required decimal MarginStatic { get; init; }
    [DefaultValue(typeof(decimal), "0")]
    public required decimal PrestigeAmount { get; init; }
    [DefaultValue(new int[0])]
    public required int[] CategoryIds { get; init; }
    [DefaultValue(new int[0])]
    public required int[] TargetIds { get; init; }
}

public record ModifierCreateResponse : ModifierDetailModel;

public record ModifierUpdateRequest {
    [FromRoute]
    public required int Id { get; init; }
    [FromBody]
    public required ModifierUpdateModel Model { get; init; }
};

public record ModifierUpdateResponse : ModifierDetailModel;

public record ModifierReadResponse : ModifierDetailModel;
