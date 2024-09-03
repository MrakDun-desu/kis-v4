namespace KisV4.Common.Models;

public record DiscountListModel(
    int Id,
    string Name,
    bool Deleted
);

public record DiscountDetailModel(
    int Id,
    string Name,
    bool Deleted,
    Page<DiscountUsageListModel> DiscountUsages
);