namespace KisV4.Common.Models;

public record DiscountUsageReadAllModel(
    int Id,
    UserReadAllModel User,
    DiscountReadAllModel Discount,
    DateTimeOffset Timestamp
);

public record DiscountUsageReadModel(
    int Id,
    UserReadAllModel User,
    DiscountReadAllModel Discount,
    DateTimeOffset Timestamp,
    IEnumerable<DiscountUsageItemModel> UsageItems
);