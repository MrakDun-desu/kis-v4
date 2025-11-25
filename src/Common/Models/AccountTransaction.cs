using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record AccountTransactionModel {
    public decimal Amount { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public int SaleTransactionId { get; init; }
}

// Requests and responses
public record AccountTransactionReadAllRequest : PagedRequest {
    public required int AccountId { get; init; }
    public DateTimeOffset? From { get; init; }
    public DateTimeOffset? To { get; init; }
}

public record AccountTransactionReadAllResponse : PagedResponse<AccountTransactionModel> {
    public required int AccountId { get; init; }
    public required DateTimeOffset From { get; init; }
    public required DateTimeOffset To { get; init; }
    public required decimal Total { get; init; }
}
