using System.ComponentModel;
using KisV4.Common.Enums;
using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
// Used in admin for list view
public record SaleItemListModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Image { get; init; }
    public required decimal MarginPercent { get; init; }
    public required decimal MarginStatic { get; init; }
    public required decimal PrestigeAmount { get; init; }
    public required PrintType PrintType { get; init; }
}

// Used in operator for listing sale items in the grid
public record SaleItemOperatorModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Image { get; init; }
    public required decimal CurrentPrice { get; init; }
    public required decimal AmountInStore { get; init; }
}

// Used in admin for detail view (composition is fetched separately)
public record SaleItemDetailModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Image { get; init; }
    public required decimal MarginPercent { get; init; }
    public required decimal MarginStatic { get; init; }
    public required decimal PrestigeAmount { get; init; }
    public required PrintType PrintType { get; init; }
    public required IEnumerable<ModifierListModel> ApplicableModifiers { get; init; }
    public required IEnumerable<CategoryModel> Categories { get; init; }
}

// Requests and responses
public record SaleItemReadAllRequest : PagedRequest {
    public string? Name { get; init; }
    public int? CategoryId { get; init; }
}

public record SaleItemReadAllResponse : PagedResponse<SaleItemListModel>;

public record SaleItemCreateRequest {
    [DefaultValue("Kofola")]
    public string Name { get; init; } = string.Empty;
    public string? Image { get; init; }
    [DefaultValue(typeof(decimal), "5")]
    public decimal MarginPercent { get; init; }
    [DefaultValue(typeof(decimal), "0")]
    public decimal MarginStatic { get; init; }
    [DefaultValue(typeof(decimal), "0")]
    public decimal PrestigeAmount { get; init; }
    [DefaultValue(PrintType.DontPrint)]
    public PrintType PrintType { get; init; }
    [DefaultValue(new int[0])]
    public int[] CategoryIds { get; init; } = [];
    [DefaultValue(new int[0])]
    public int[] ModifierIds { get; init; } = [];
}

public record SaleItemCreateResponse : SaleItemDetailModel;

public record SaleItemUpdateRequest {
    [DefaultValue("Kofola")]
    public string Name { get; init; } = string.Empty;
    public string? Image { get; init; }
    [DefaultValue(typeof(decimal), "5")]
    public decimal MarginPercent { get; init; }
    [DefaultValue(typeof(decimal), "0")]
    public decimal MarginStatic { get; init; }
    [DefaultValue(typeof(decimal), "0")]
    public decimal PrestigeAmount { get; init; }
    [DefaultValue(PrintType.DontPrint)]
    public PrintType PrintType { get; init; }
    [DefaultValue(new int[0])]
    public int[] CategoryIds { get; init; } = [];
    [DefaultValue(new int[0])]
    public int[] ModifierIds { get; init; } = [];
}

public record SaleItemUpdateResponse : SaleItemDetailModel;

public record SaleItemReadResponse : SaleItemDetailModel;
