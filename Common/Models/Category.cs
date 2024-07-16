namespace KisV4.Common.Models;

public record CategoryCreateModel(string Name);

public record CategoryModel(int Id, string Name);

public record CategoryUpdateModel(string? Name);
