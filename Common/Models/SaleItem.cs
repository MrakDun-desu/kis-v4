namespace KisV4.Common.Models;

public record SaleItemCreateModel(
    string Name,
    string Image,
    IEnumerable<int> CategoryIds,
    bool ShowOnWeb
);

public record SaleItemListModel(
    int Id,
    string Name,
    string Image,
    bool Deleted,
    bool ShowOnWeb
);

public record SaleItemDetailModel(
    int Id,
    string Name,
    string Image,
    bool Deleted,
    bool ShowOnWeb,
    IEnumerable<CategoryListModel> Categories,
    IEnumerable<CompositionListModel> Composition,
    IEnumerable<ModifierListModel> AvailableModifiers,
    IEnumerable<CostListModel> Costs,
    IEnumerable<CostListModel>? CurrentCosts = null,
    IEnumerable<StoreAmountSaleItemListModel>? StoreAmounts = null
)
{
    public IEnumerable<CostListModel> CurrentCosts = 
        CurrentCosts ?? new List<CostListModel>();
    public IEnumerable<StoreAmountSaleItemListModel> StoreAmounts = 
        StoreAmounts ?? new List<StoreAmountSaleItemListModel>();
}