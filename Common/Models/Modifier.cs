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
    IEnumerable<CostModel> Costs,
    IEnumerable<CompositionReadAllModel> Composition,
    IEnumerable<ModifierReadAllModel> AvailableModifiers,
    bool ShowOnWeb
);

public record ModifierUpdateModel(
    string? Name,
    string? Image,
    IEnumerable<CategoryReadAllModel>? Categories,
    bool? ShowOnWeb
);
