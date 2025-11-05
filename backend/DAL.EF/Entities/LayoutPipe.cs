namespace KisV4.DAL.EF.Entities;

public record LayoutPipe : LayoutItem {
    public int TargetId { get; init; }
    public Pipe? Target { get; set; }
}
