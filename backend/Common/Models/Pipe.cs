using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record PipeListModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
}

public record PipeDetailModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required SaleItemReadAllResponse SaleItems { get; init; }
    public required ContainerReadAllResponse Containers { get; init; }
}

// Requests and responses
public record PipeReadAllResponse : CollectionResponse<PipeListModel>;

public record PipeCreateRequest {
    public required string Name { get; init; }
}

public record PipeCreateResponse : PipeListModel;

public record PipeUpdateRequest {
    public required string Name { get; init; }
}

public record PipeUpdareResponse : PipeListModel;

public record PipeReadResponse : PipeDetailModel;
