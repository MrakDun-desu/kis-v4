namespace KisV4.Common.Models;

public record StoreItemCreateModel(
    string Name,
    string Image,
    IEnumerable<CategoryReadAllModel> Categories,
    string UnitName,
    bool BarmanCanStock,
    bool IsContainerItem
);

public record StoreItemReadAllModel(
    int Id,
    string Name,
    string Image,
    IEnumerable<CategoryReadAllModel> Categories,
    string UnitName,
    bool BarmanCanStock,
    bool IsContainerItem
);

public record StoreItemReadModel(
    int Id,
    string Name,
    string Image,
    IEnumerable<CategoryReadAllModel> Categories,
    IEnumerable<CostModel> Costs,
    string UnitName,
    bool BarmanCanStock,
    bool IsContainerItem
);

public record StoreItemUpdateModel(
    string? Name,
    string? Image,
    IEnumerable<CategoryReadAllModel>? Categories,
    string UnitName,
    bool BarmanCanStock,
    bool IsContainerItem
);
