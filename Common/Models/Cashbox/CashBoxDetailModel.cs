using KisV4.Common.Models.CurrencyChange;

namespace KisV4.Common.Models.CashBox;

// TODO Figure out what to do with the stock takings.
// Include all or just store the latest one?
public record CashBoxDetailModel(
    int Id,
    string Name,
    ICollection<CurrencyChangeModel> CurrencyChanges
);
