using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record ModificationModel {
    public required int Amount { get; init; }
    public required string ModifierName { get; init; }
    public required decimal PriceChange { get; init; }
}

// Requests and responses
public record ModificationCreateRequest {
    public required int Amount { get; init; }
    public required int ModifierId { get; init; }
}
