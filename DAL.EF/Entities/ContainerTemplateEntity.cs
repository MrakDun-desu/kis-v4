using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

public record ContainerTemplateEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [Precision(11,2)]
    public decimal Amount { get; set; }
    public bool Deleted { get; set; }
    public int ContainedItemId { get; set; }
    public virtual StoreItemEntity? ContainedItem { get; set; }
    public virtual ICollection<ContainerEntity> Instances { get; private set; } = new List<ContainerEntity>();
}