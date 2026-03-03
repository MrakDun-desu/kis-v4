using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record StoreItemAmountModel {
    public required decimal Amount { get; init; }
    public required int StoreItemId { get; init; }
    public required int StoreId { get; init; }
}

// Requests and responses
public record StoreItemAmountReadAllRequest : PagedRequest {
    public required int StoreId { get; init; }
}

public record StoreItemAmountReadAllResponse : PagedResponse<StoreItemAmountModel>;
