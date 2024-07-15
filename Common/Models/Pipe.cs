namespace KisV4.Common.Models;

public record PipeCreateModel(string Name);

public record PipeModel(int Id, string Name);

public record PipeUpdateModel(string? Name);
