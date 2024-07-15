using KisV4.Api.Common.Models.CurrencyChange;

namespace KisV4.Api.Common.Models.CashBox;

// TODO Figure out what to do with the stock takings.
// Include all or just store the latest one?
public record CashBoxDetailModel(
    int Id,
    string Name,
    ICollection<CurrencyChangeModel> CurrencyChanges
);
