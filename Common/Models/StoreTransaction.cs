using KisV4.Common.Enums;

namespace KisV4.Common.Models;

public record StoreTransactionCreateModel(
    IEnumerable<StoreTransactionItemCreateModel> StoreTransactionItems,
    TransactionReason TransactionReason,
    int StoreId,
    int? DestinationStoreId,
    string? Note = null
);

public record StoreTransactionListModel(
    int Id,
    string? Note,
    UserListModel ResponsibleUser,
    DateTimeOffset Timestamp,
    bool Cancelled,
    TransactionReason TransactionReason,
    int? SaleTransactionId
);

public record StoreTransactionDetailModel(
    int Id,
    string? Note,
    UserListModel ResponsibleUser,
    DateTimeOffset Timestamp,
    bool Cancelled,
    TransactionReason TransactionReason,
    SaleTransactionListModel? SaleTransaction,
    IEnumerable<StoreTransactionItemListModel> StoreTransactionItems
);
