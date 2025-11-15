using Audit.EntityFramework;

namespace KisV4.DAL.EF.Entities;

public record Composite {
    public int Id { get; init; }
    public required string Name { get; set; }
    public string? Image { get; set; }
    public bool Hidden { get; set; }
    public decimal MarginPercent { get; set; }
    public decimal MarginStatic { get; set; }
    public decimal PrestigeAmount { get; set; }

    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<Composition> Compositions { get; set; } = [];
}
