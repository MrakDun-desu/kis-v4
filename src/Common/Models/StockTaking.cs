using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record StockTakingModel {
    public required DateTimeOffset Timestamp { get; init; }
    public required int CashBoxId { get; init; }
    public required UserListModel User { get; init; }
}

// Requests and responses
public record StockTakingReadAllRequest : PagedRequest {
    public required int CashBoxId { get; init; }
}

public record StockTakingReadAllResponse : PagedResponse<StockTakingModel>;

public record StockTakingCreateResponse : StockTakingModel;

