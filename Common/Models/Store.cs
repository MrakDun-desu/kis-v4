namespace KisV4.Common.Models;

public record StoreCreateModel(string Name);

public record StoreListModel(int Id, string Name, bool Deleted);

public record StoreDetailModel(
    int Id,
    string Name,
    bool Deleted,
    Page<StoreItemAmountListModel> StoreItemAmounts,
    Page<SaleItemAmountListModel> SaleItemAmounts
);
