namespace KisV4.Common.Models;

public record StoreItemCreateModel(
    string Name,
    string Image,
    IEnumerable<int> CategoryIds,
    string UnitName,
    bool BarmanCanStock,
    bool IsContainerItem
);

public record StoreItemListModel(
    int Id,
    string Name,
    string Image,
    bool Deleted,
    string UnitName,
    bool BarmanCanStock,
    bool IsContainerItem,
    IEnumerable<CostListModel> CurrentCosts
);

public record StoreItemDetailModel(
    int Id,
    string Name,
    string Image,
    bool Deleted,
    string UnitName,
    bool BarmanCanStock,
    bool IsContainerItem,
    IEnumerable<CategoryListModel> Categories,
    IEnumerable<CompositionListModel> Composition,
    IEnumerable<CostListModel> Costs,
    IEnumerable<StoreAmountListModel> StoreAmounts
);
