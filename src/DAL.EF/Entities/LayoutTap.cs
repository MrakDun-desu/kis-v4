namespace KisV4.DAL.EF.Entities;

public record LayoutTap : LayoutItem {
    public int TargetId { get; init; }
    public Tap? Target { get; set; }
}
