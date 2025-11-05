namespace KisV4.DAL.EF.Entities;

public abstract record Transaction {
    public int Id { get; init; }
    public string? Note { get; set; }
    public DateTimeOffset StartedAt { get; init; }
    public DateTimeOffset? CancelledAt { get; set; }

    public int StartedById { get; init; }
    public User? StartedBy { get; init; }
    public int? CancelledById { get; init; }
    public User? CancelledBy { get; init; }
}
