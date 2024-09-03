namespace KisV4.Common.Models;

public record UserListModel(
    int Id,
    string UserName,
    bool Deleted
);

public record UserDetailModel(
    int Id,
    string UserName,
    IEnumerable<TotalCurrencyChangeListModel> CurrencyAmounts,
    Page<CurrencyChangeListModel> CurrencyChanges,
    Page<DiscountUsageListModel> DiscountUsages
);