namespace KisV4.Common.Models;

public record UserListModel(
    int Id,
    string UserName,
    bool Deleted
);

public record UserDetailModel(
    int Id,
    string UserName,
    bool Deleted,
    IEnumerable<TotalCurrencyChangeListModel>? CurrencyAmounts = null,
    Page<CurrencyChangeListModel>? CurrencyChanges = null,
    Page<DiscountUsageListModel>? DiscountUsages = null
)
{
    public IEnumerable<TotalCurrencyChangeListModel> CurrencyAmounts =
        CurrencyAmounts ?? Array.Empty<TotalCurrencyChangeListModel>();

    public Page<CurrencyChangeListModel> CurrencyChanges =
        CurrencyChanges ?? Page<CurrencyChangeListModel>.Empty;

    public Page<DiscountUsageListModel> DiscountUsages =
        DiscountUsages ?? Page<DiscountUsageListModel>.Empty;
}