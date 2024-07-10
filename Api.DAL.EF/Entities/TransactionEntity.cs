namespace Api.DAL.EF.Entities;

/// <summary>
/// Represents a general transaction.
/// </summary>
public record TransactionEntity {
    public required int Id { get; set; }

    public int? ResponsibleUserId { get; set; }
    public UserAccountEntity? ResponsibleUser { get; set; }
    public DateTime Timestamp { get; set; }
    public bool Cancelled { get; set; }
}
