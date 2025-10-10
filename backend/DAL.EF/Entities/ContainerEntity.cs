using Microsoft.EntityFrameworkCore;

namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents a container that holds some kind of StoreItem. Intended use is for beer kegs.
/// </summary>
public record ContainerEntity : StoreEntity {
    public int TemplateId { get; init; }
    public virtual ContainerTemplateEntity? Template { get; set; }

    /// <summary>
    ///     Timestamp for when container was first opened (first time the PipeId was set to not null).
    /// </summary>
    [Precision(0)]
    public DateTimeOffset OpenSince { get; set; }

    public int? PipeId { get; set; }

    /// <summary>
    ///     Pipe that the container is currently active at, if the container is active.
    ///     If container isn't active, this should be null.
    /// </summary>
    public virtual PipeEntity? Pipe { get; set; }
}
