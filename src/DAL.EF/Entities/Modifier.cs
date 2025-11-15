namespace KisV4.DAL.EF.Entities;

public record Modifier : Composite {
    public ICollection<Modification> Modifications { get; init; } = [];
    public ICollection<SaleItem> Targets { get; init; } = [];
}
