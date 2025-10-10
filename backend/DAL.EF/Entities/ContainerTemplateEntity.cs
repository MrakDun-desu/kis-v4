using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Entity that serves as a template for containers. When creating containers, it should be done only
///     through "instantiating" these templates - name, starting amount, and contained item should be the same.
/// </summary>
public record ContainerTemplateEntity {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Initial amount of store item that is stored within this container.
    /// </summary>
    [Precision(11, 2)]
    public decimal Amount { get; set; }

    public bool Deleted { get; set; }
    public int ContainedItemId { get; set; }

    /// <summary>
    ///     Item that will be stored in the instances of this template. Has to have the flag of container item.
    /// </summary>
    public virtual StoreItemEntity? ContainedItem { get; set; }

    /// <summary>
    ///     Instances of this container - individual container entities.
    /// </summary>
    public virtual ICollection<ContainerEntity> Instances { get; private set; } = [];
}
