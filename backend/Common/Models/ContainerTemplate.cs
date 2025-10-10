namespace KisV4.Common.Models;

public record ContainerTemplateCreateModel(
    string Name,
    int ContainedItemId,
    decimal Amount
);

public record ContainerTemplateListModel(
    int Id,
    string Name,
    decimal Amount,
    bool Deleted,
    StoreItemListModel ContainedItem
);
