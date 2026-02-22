using System.Text.Json.Serialization;
using KisV4.Common.Enums;
using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
[JsonDerivedType(typeof(LayoutSaleItemModel), LayoutItemType.SaleItem)]
[JsonDerivedType(typeof(LayoutLinkModel), LayoutItemType.Layout)]
[JsonDerivedType(typeof(LayoutPipeModel), LayoutItemType.Pipe)]
public abstract record LayoutItemModel {
    public required int X { get; init; }
    public required int Y { get; init; }
    public required string Type { get; init; }
}

public record LayoutSaleItemModel : LayoutItemModel {
    public required SaleItemListModel Target { get; init; }
}

public record LayoutLinkModel : LayoutItemModel {
    public required LayoutListModel Target { get; init; }
}

public record LayoutPipeModel : LayoutItemModel {
    public required PipeListModel Target { get; init; }
}

// Requests and responses
public record LayoutItemReadAllResponse : CollectionResponse<LayoutItemModel>;

public record LayoutItemCreateRequest {
    public required int X { get; init; }
    public required int Y { get; init; }
    public required int TargetId { get; init; }
    public required string Type { get; init; }
}
