namespace KisV4.Common.Models;

public record ContainerCreateModel(
    string Name,
    int ContainedItemId,
    decimal ItemAmount);

public record ContainerReadAllModel(
    int Id,
    string Name
);

public record ContainerUpdateModel(
    string? Name,
    int? PipeId
);