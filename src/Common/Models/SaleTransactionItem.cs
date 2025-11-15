using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record SaleTransactionItemModel {
    public required int Id { get; init; }
    public required int ItemAmount { get; init; }
    public required SaleItemListModel SaleItem { get; init; }
    public required int SaleTransactionId { get; init; }
    public required ModificationReadAllResponse Modifications { get; init; }
}

// Requests and responses
public record SaleTransactionItemCreateRequest {
    public required int ItemAmount { get; init; }
    public required int SaleItemId { get; init; }
    public required ModificationCreateRequest[] Modifications { get; init; }
}

public record SaleTransactionItemReadAllResponse : CollectionResponse<SaleTransactionItemModel>;

