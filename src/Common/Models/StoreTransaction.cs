using KisV4.Common.Enums;
using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record StoreTransactionListModel {
    public required int Id { get; init; }
    public required string? Note { get; init; }
    public required DateTimeOffset StartedAt { get; init; }
    public required DateTimeOffset? CancelledAt { get; init; }
    public required UserListModel StartedBy { get; init; }
    public required UserListModel? CancelledBy { get; init; }
    public required StoreTransactionReason Reason { get; init; }
    public required int? SaleTransactionId { get; init; }
}

public record StoreTransactionDetailModel {
    public required int Id { get; init; }
    public required string? Note { get; init; }
    public required DateTimeOffset StartedAt { get; init; }
    public required DateTimeOffset? CancelledAt { get; init; }
    public required UserListModel StartedBy { get; init; }
    public required UserListModel? CancelledBy { get; init; }
    public required StoreTransactionReason Reason { get; init; }
    public required int? SaleTransactionId { get; init; }
    public required StoreTransactionItemModel[] StoreTransactionItems { get; init; }
}

// Requests and responses
public record StoreTransactionReadAllRequest : PagedRequest {
    public DateTimeOffset? From { get; init; }
    public DateTimeOffset? To { get; init; }
    public bool OnlySelfCancellable { get; init; } = true;
}

public record StoreTransactionReadAllResponse : PagedResponse<StoreTransactionListModel>;

public record StoreTransactionCreateRequest {
    public string? Note { get; init; }
    public required StoreTransactionItemCreateRequest[] StoreTransactionItems { get; init; }
    public required StoreTransactionReason Reason { get; init; }
    public required int StoreId { get; init; }
    public int? SourceStoreId { get; init; }
    public bool UpdateCosts { get; init; }
}

public record StoreTransactionCreateResponse : StoreTransactionDetailModel;

public record StoreTransactionReadResponse : StoreTransactionDetailModel;

public record StoreTransactionDeleteRequest {
    public bool UpdateCosts { get; init; }
}
