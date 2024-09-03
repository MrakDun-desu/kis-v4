namespace KisV4.Common.Models;

public record ContainerCreateModel(
    int TemplateId,
    int PipeId
);

public record ContainerListModel(
    int Id,
    DateTimeOffset OpenSince,
    PipeListModel? Pipe,
    bool Deleted,
    ContainerTemplateListModel Template,
    decimal CurrentAmount
);

public record ContainerPatchModel(
    int? PipeId
);