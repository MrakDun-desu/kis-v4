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
    public required string Name { get; init; }
}

public record CategoryCreateResponse : CategoryModel;

public record CategoryUpdateRequest {
    public required string Name { get; init; }
}
