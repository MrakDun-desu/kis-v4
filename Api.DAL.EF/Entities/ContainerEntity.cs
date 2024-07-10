using Microsoft.EntityFrameworkCore;

namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a container that holds some kind of StoreItem. Original use is for beer kegs.
/// </summary>
public record ContainerEntity : StoreEntity {
    public int ContainedItemId { get; set; }
    public StoreItemEntity? ContainedItem { get; set; }
    public DateTime OpenSince { get; set; }
    public bool WrittenOff { get; set; }
    [Precision(11,2)]
    public decimal MaxAmount { get; set; }
    [Precision(11,2)]
    public decimal CurrentAmount { get; set; }
    public int? PipeId { get; set; }
    public PipeEntity? Pipe { get; set; }
}
