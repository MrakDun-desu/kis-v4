namespace KisV4.Common.Models;

public record ContainerCreateModel(
    int TemplateId,
    int PipeId
);

public record ContainerReadAllModel(
    int Id,
    DateTimeOffset? OpenSince,
    PipeReadAllModel Pipe,
    bool WrittenOff,
    ContainerTemplateReadAllModel Template
);

public record ContainerUpdateModel(
    int? PipeId
);