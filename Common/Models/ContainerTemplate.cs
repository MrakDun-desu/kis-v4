namespace KisV4.Common.Models;

public record ContainerTemplateCreateModel(
    string Name,
    int ContainedItemId,
    decimal Amount
);

public record ContainerTemplateReadAllModel(
    int Id,
    string Name,
    decimal Amount,
    bool Deleted,
    StoreItemReadAllModel ContainedItem
);

public record ContainerTemplateUpdateModel(
    int Id,
    string Name,
    int ContainedItemId,
    decimal Amount
);