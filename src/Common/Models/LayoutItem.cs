using System.ComponentModel;
using System.Text.Json.Serialization;
using KisV4.Common.Enums;
using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(LayoutSaleItemModel), LayoutItemType.SaleItem)]
[JsonDerivedType(typeof(LayoutLinkModel), LayoutItemType.Layout)]
[JsonDerivedType(typeof(LayoutPipeModel), LayoutItemType.Pipe)]
public abstract record LayoutItemModel {
    public required int X { get; init; }
    public required int Y { get; init; }
}

public record LayoutSaleItemModel : LayoutItemModel {
    public required SaleItemOperatorModel Target { get; init; }
}

public record LayoutLinkModel : LayoutItemModel {
    public required LayoutListModel Target { get; init; }
}

public record LayoutPipeModel : LayoutItemModel {
    public required PipeListModel Target { get; init; }
}

// Requests and responses
public record LayoutItemCreateRequest {
    public int X { get; init; }
    public int Y { get; init; }
    public int TargetId { get; init; }
    [DefaultValue(LayoutItemType.SaleItem)]
    public string Type { get; init; } = string.Empty;
}
