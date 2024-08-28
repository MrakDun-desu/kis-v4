namespace KisV4.Common.Models;

public record CurrencyReadAllModel(int Id, string Name, string ShortName);

public record CurrencyCreateModel(string Name, string ShortName);

public record CurrencyUpdateModel(int Id, string Name, string ShortName);