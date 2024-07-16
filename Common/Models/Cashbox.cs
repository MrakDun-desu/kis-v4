namespace KisV4.Common.Models;

public record CashBoxCreateModel(string Name);
public record CashBoxUpdateModel(string? Name);
public record CashBoxReadAllModel(int Id, string Name);
// TODO Figure out what to do with the stock takings.
// Include all or just store the latest one?
public record CashBoxReadModel(
    int Id,
    string Name,
    ICollection<CurrencyChangeModel> CurrencyChanges
);
