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
    bool ShowOnWeb,
    IEnumerable<CostListModel>? CurrentCosts = null
)
{
    public IEnumerable<CostListModel> CurrentCosts = new List<CostListModel>();
}

public record SaleItemDetailModel(
    int Id,
    string Name,
    string Image,
    bool Deleted,
    bool ShowOnWeb,
    IEnumerable<CategoryListModel> Categories,
    IEnumerable<CostListModel> Costs,
    IEnumerable<CompositionListModel> Composition,
    IEnumerable<ModifierListModel> AvailableModifiers,
    IEnumerable<StoreAmountListModel>? StoreAmounts = null
)
{
    public IEnumerable<StoreAmountListModel> StoreAmounts = new List<StoreAmountListModel>();
}