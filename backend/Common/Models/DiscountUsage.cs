namespace KisV4.Common.Models;

public record DiscountUsageCreateModel(
    int DiscountId,
    string UserName,
    int SaleTransactionId
);

public record DiscountUsageListModel(
    int Id,
    UserListModel User,
    DiscountListModel Discount,
    DateTimeOffset Timestamp
);

public record DiscountUsageDetailModel(
    int Id,
    UserListModel User,
    DiscountListModel Discount,
    DateTimeOffset Timestamp,
    IEnumerable<DiscountUsageItemListModel> UsageItems
);