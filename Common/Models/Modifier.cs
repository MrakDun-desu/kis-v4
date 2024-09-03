namespace KisV4.Common.Models;

public record ModifierCreateModel(
    string Name,
    string Image,
    bool ShowOnWeb,
    int ModificationTargetId
);

public record ModifierListModel(
    int Id,
    string Name,
    string Image,
    bool ShowOnWeb,
    int ModificationTargetId,
    bool Deleted,
    IEnumerable<CostListModel> CurrentCosts
);

public record ModifierDetailModel(
    int Id,
    string Name,
    string Image,
    bool ShowOnWeb,
    SaleItemListModel ModificationTarget,
    bool Deleted,
    IEnumerable<CostListModel> Costs,
    IEnumerable<CompositionListModel> Composition
);