using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

[PrimaryKey(nameof(LayoutId), nameof(X), nameof(Y))]
public abstract record LayoutItem {
    public int X { get; init; }
    public int Y { get; init; }
    public required string Type { get; init; }

    public int LayoutId { get; init; }
    public Layout? Layout { get; set; }
}

