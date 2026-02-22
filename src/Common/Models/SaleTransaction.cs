using KisV4.Common.Enums;
using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record SaleTransactionListModel {
    public required int Id { get; init; }
    public required string? Note { get; init; }
    public required DateTimeOffset StartedAt { get; init; }
    public required DateTimeOffset? CancelledAt { get; init; }
    public required UserListModel StartedBy { get; init; }
    public required UserListModel? CancelledBy { get; init; }
    public required bool Open { get; init; }
    public required UserListModel? OpenedBy { get; init; }
    public required TransactionReason Reason { get; init; }
}

public record SaleTransactionDetailModel {
    public required int Id { get; init; }
    public required string? Note { get; init; }
    public required DateTimeOffset StartedAt { get; init; }
    public required DateTimeOffset? CancelledAt { get; init; }
    public required UserListModel StartedBy { get; init; }
    public required UserListModel? CancelledBy { get; init; }
    public required bool Open { get; init; }
    public required UserListModel? OpenedBy { get; init; }

    public required IEnumerable<SaleTransactionItemModel> SaleTransactionItems { get; init; }
    public required IEnumerable<AccountTransactionModel> AccountTransactions { get; init; }
    public required IEnumerable<StoreTransactionListModel> StoreTransactions { get; init; }
}

// Requests and responses
public record SaleTransactionReadAllRequest : PagedRequest {
    public DateTimeOffset? From { get; init; }
    public DateTimeOffset? To { get; init; }
    public bool OnlySelfCancellable { get; init; } = true;
}

public record SaleTransactionReadAllResponse : PagedResponse<SaleTransactionListModel> {
    public DateTimeOffset From { get; init; }
    public DateTimeOffset To { get; init; }
}

public record SaleTransactionCheckPriceRequest {
    public required SaleTransactionItemCreateRequest[] SaleTransactionItems { get; init; }
}

public record SaleTransactionCheckPriceResponse {
    public required SaleTransactionItemReadAllResponse SaleTransactionItems { get; init; }
}

public record SaleTransactionCreateRequest {
    public string? Note { get; init; }
    public required int StoreId { get; init; }
    public required int CashBoxId { get; init; }
    public required int CustomerId { get; init; }
    public required decimal PaidAmount { get; init; }
    public required SaleTransactionItemCreateRequest[] SaleTransactionItems { get; init; }
}

public record SaleTransactionCreateResponse : SaleTransactionDetailModel;

public record SaleTransactionOpenRequest {
    public string? Note { get; init; }
    public required int StoreId { get; init; }
    public required bool AssignToCreator { get; init; }
    public required SaleTransactionItemCreateRequest[] SaleTransactionItems { get; init; }
}

public record SaleTransactionOpenResponse : SaleTransactionDetailModel;

public record SaleTransactionUpdateRequest {
    public string? Note { get; init; }
    public required int StoreId { get; init; }
    public required SaleTransactionItemCreateRequest[] SaleTransactionItems { get; init; }
}

public record SaleTransactionUpdateResponse : SaleTransactionDetailModel;

public record SaleTransactionCloseRequest {
    public string? Note { get; init; }
    public required int CashBoxId { get; init; }
    public required int CustomerId { get; init; }
    public required decimal PaidAmount { get; init; }
}

public record SaleTransactionCloseResponse : SaleTransactionDetailModel;

public record SaleTransactionReadResponse : SaleTransactionDetailModel;
