using KisV4.Common.Enums;

namespace KisV4.Common.Models;

public record StoreTransactionCreateModel(
    IEnumerable<StoreTransactionItemCreateModel> StoreTransactionItems,
    TransactionReason TransactionReason,
    int StoreId,
    int? DestinationStoreId
);

public record StoreTransactionListModel(
    int Id,
    UserListModel ResponsibleUser,
    DateTimeOffset Timestamp,
    bool Cancelled,
    TransactionReason TransactionReason,
    int? SaleTransactionId
);

public record StoreTransactionDetailModel(
    int Id,
    UserListModel ResponsibleUser,
    DateTimeOffset Timestamp,
    bool Cancelled,
    TransactionReason TransactionReason,
    SaleTransactionListModel? SaleTransaction,
    IEnumerable<StoreTransactionItemListModel> StoreTransactionItems
);
