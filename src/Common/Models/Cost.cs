using System.ComponentModel;

namespace KisV4.Common.Models;

// Base models
public record CostModel {
    public required decimal Amount { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
    public required string Description { get; init; }
    public required int StoreItemId { get; init; }
    public required UserListModel User { get; init; }
}

// Requests and responses
public record CostCreateRequest {
    public int StoreItemId { get; init; }
    public decimal Amount { get; init; }
    [DefaultValue("Nová cena")]
    public string Description { get; init; } = string.Empty;
}

public record CostCreateResponse : CostModel;
