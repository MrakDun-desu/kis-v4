using System.ComponentModel;
using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record CategoryModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
}

// Requests and responses
public record CategoryReadAllResponse : CollectionResponse<CategoryModel>;

public record CategoryCreateRequest {
    [DefaultValue("Pivo")]
    public string Name { get; init; } = string.Empty;
}

public record CategoryCreateResponse : CategoryModel;

public record CategoryUpdateRequest {
    [DefaultValue("Pivo")]
    public string Name { get; init; } = string.Empty;
}
