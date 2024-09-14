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
    bool IsContainerItem
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
    IEnumerable<CostListModel> Costs,
    IEnumerable<CostListModel>? CurrentCosts = null,
    IEnumerable<StoreAmountStoreItemListModel>? StoreAmounts = null
)
{
    public IEnumerable<CostListModel> CurrentCosts = 
        CurrentCosts ?? new List<CostListModel>();
    public IEnumerable<StoreAmountStoreItemListModel> StoreAmounts = 
        StoreAmounts ?? new List<StoreAmountStoreItemListModel>();
}
