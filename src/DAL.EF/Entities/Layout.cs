namespace KisV4.DAL.EF.Entities;

public record Layout {
    public int Id { get; init; }
    public required string Name { get; set; }
    public string? Image { get; set; }
    public bool TopLevel { get; set; }

    public ICollection<LayoutItem> LayoutItems { get; set; } = [];
}
