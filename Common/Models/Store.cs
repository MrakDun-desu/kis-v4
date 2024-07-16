namespace KisV4.Common.Models;

public record StoreCreateModel(string Name);

public record StoreReadAllModel(int Id, string Name);

public record StoreUpdateModel(string? Name);
