namespace KisV4.Common.Models;

public record ContainerCreateModel(
    int TemplateId,
    int PipeId
);

public record ContainerReadAllModel(
    int Id,
    DateTimeOffset OpenSince,
    PipeReadAllModel? Pipe,
    bool Deleted,
    ContainerTemplateReadAllModel Template,
    decimal CurrentAmount
);

public record ContainerUpdateModel(
    int Id,
    int? PipeId
);