namespace KisV4.Common.Models;

public record SaleItemCreateModel(
    string Name,
    string Image,
    IEnumerable<CategoryReadAllModel> Categories,
    bool ShowOnWeb
);

public record SaleItemReadAllModel(
    int Id,
    string Name,
    string Image,
    IEnumerable<CategoryReadAllModel> Categories,
    bool ShowOnWeb
);

public record SaleItemReadModel(
    int Id,
    string Name,
    string Image,
    IEnumerable<CategoryReadAllModel> Categories,
    IEnumerable<CostModel> Costs,
    IEnumerable<CompositionReadAllModel> Composition,
    IEnumerable<ModifierReadAllModel> AvailableModifiers,
    bool ShowOnWeb
);

public record SaleItemUpdateModel(
    string? Name,
    string? Image,
    IEnumerable<CategoryReadAllModel>? Categories,
    bool? ShowOnWeb
);
