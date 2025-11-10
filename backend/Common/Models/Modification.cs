using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record ModificationModel {
    public required int Amount { get; init; }
    public required ModifierListModel Modifier { get; init; }
    public required int SaleTransactionItemId { get; init; }
}

// Requests and responses
public record ModificationReadAllRequest {
    public required int SaleTransactionItemId { get; init; }
}

public record ModificationReadAllResponse : CollectionResponse<ModificationModel>;

public record ModificationCreateRequest {
    public required int Amount { get; init; }
    public required int ModifierId { get; init; }
}
