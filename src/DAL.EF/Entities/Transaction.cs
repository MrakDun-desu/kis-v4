using KisV4.Common.Enums;

namespace KisV4.DAL.EF.Entities;

public abstract record Transaction {
    public int Id { get; init; }
    public required string? Note { get; set; }
    public required DateTimeOffset StartedAt { get; init; }
    public DateTimeOffset? CancelledAt { get; set; }
    public required TransactionReason Reason { get; init; }

    public required int StartedById { get; init; }
    public User? StartedBy { get; init; }
    public int? CancelledById { get; init; }
    public User? CancelledBy { get; init; }
}
