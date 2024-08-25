namespace KisV4.Common.Models;

public record ModifierCreateModel(
    int ModificationTargetId,
    string Name,
    string Image,
    IEnumerable<CategoryReadAllModel> Categories,
    bool ShowOnWeb
);

public record ModifierReadAllModel(
    int Id,
    int ModificationTargetId,
    string Name,
    string Image,
    IEnumerable<CategoryReadAllModel> Categories,
    bool ShowOnWeb
);

public record ModifierReadModel(
    int Id,
    int ModificationTargetId,
    string Name,
    string Image,
    IEnumerable<CategoryReadAllModel> Categories,
    IEnumerable<CostReadAllModel> Costs,
    IEnumerable<CompositionReadAllModel> Composition,
    bool ShowOnWeb
);

public record ModifierUpdateModel(
    string? Name,
    string? Image,
    IEnumerable<CategoryReadAllModel>? Categories,
    bool? ShowOnWeb
);