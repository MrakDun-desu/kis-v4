using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record CompositeAmountModel {
    public required decimal Amount { get; init; }
    public required int CompositeId { get; init; }
    public required int StoreId { get; init; }
}

// Requests and responses
public record CompositeAmountReadAllRequest : PagedRequest {
    public required int StoreId { get; init; }
}

public record CompositeAmountReadAllResponse : PagedResponse<CompositeAmountModel>;

