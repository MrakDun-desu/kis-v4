namespace KisV4.Common.Models;

public record SaleTransactionCreateModel(
    IEnumerable<SaleTransactionItemCreateModel> SaleTransactionItems,
    int StoreId,
    string ClientUserName
);

public record SaleTransactionListModel(
    int Id,
    UserListModel ResponsibleUser,
    DateTimeOffset Timestamp,
    bool Cancelled
);

public record SaleTransactionDetailModel(
    int Id,
    UserListModel ResponsibleUser,
    DateTimeOffset Timestamp,
    bool Cancelled,
    IEnumerable<SaleTransactionItemListModel> SaleTransactionItems,
    IEnumerable<StoreTransactionListModel> StoreTransactions,
    IEnumerable<CurrencyChangeListModel> CurrencyChanges
);

