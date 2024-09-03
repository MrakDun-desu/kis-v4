namespace KisV4.Common.Models;

public record CurrencyCreateModel(string Name, string ShortName);

public record CurrencyListModel(int Id, string Name, string ShortName);