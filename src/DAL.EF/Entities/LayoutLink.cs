namespace KisV4.DAL.EF.Entities;

public record LayoutLink : LayoutItem {
    public int TargetId { get; init; }
    public Layout? Target { get; set; }
}
