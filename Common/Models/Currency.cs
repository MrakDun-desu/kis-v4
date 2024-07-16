namespace KisV4.Common.Models;

public record CurrencyReadAllModel(int Id, string Name);
public record CurrencyCreateModel(string Name);
public record CurrencyUpdateModel(string? Name);
