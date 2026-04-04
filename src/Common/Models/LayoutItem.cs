using System.ComponentModel;
using System.Text.Json.Serialization;
using KisV4.Common.Enums;

namespace KisV4.Common.Models;

// Base models
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(LayoutSaleItemModel), nameof(LayoutItemType.SaleItem))]
[JsonDerivedType(typeof(LayoutLinkModel), nameof(LayoutItemType.Layout))]
[JsonDerivedType(typeof(LayoutPipeModel), nameof(LayoutItemType.Tap))]
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
    public required TapListModel Target { get; init; }
}

// Requests and responses
public record LayoutItemCreateRequest {
    public required int X { get; init; }
    public required int Y { get; init; }
    public required int TargetId { get; init; }
    [DefaultValue(LayoutItemType.SaleItem)]
    public required LayoutItemType Type { get; init; }
}
