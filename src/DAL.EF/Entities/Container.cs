using KisV4.Common.Enums;

namespace KisV4.DAL.EF.Entities;

public record Container {
    public int Id { get; init; }
    public required decimal Amount { get; set; }
    public ContainerState State { get; set; }

    public required int TemplateId { get; init; }
    public ContainerTemplate? Template { get; set; }
    public int? PipeId { get; set; }
    public Pipe? Pipe { get; set; }
    public required int StoreId { get; set; }
    public Store? Store { get; set; }
    public ICollection<ContainerChange> ContainerChanges { get; init; } = [];
}
