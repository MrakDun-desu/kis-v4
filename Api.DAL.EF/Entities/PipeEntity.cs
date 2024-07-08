namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a pipe that a container can be active in.
/// </summary>
public record PipeEntity {
    public required int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
