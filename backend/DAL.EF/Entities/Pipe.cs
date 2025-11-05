namespace KisV4.DAL.EF.Entities;

public record Pipe {
    public int Id { get; init; }
    public required string Name { get; set; }

    public ICollection<Container> Containers { get; init; } = [];
}
