namespace KisV4.Common.Models;

public record CategoryCreateModel(string Name);

public record CategoryReadAllModel(int Id, string Name);

public record CategoryUpdateModel(string? Name);
