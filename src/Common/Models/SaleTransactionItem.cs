using System.ComponentModel;
using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record SaleTransactionItemModel {
    public required int Id { get; init; }
    public required int ItemAmount { get; init; }
    public required SaleItemListModel SaleItem { get; init; }
    public required int SaleTransactionId { get; init; }
    public required IEnumerable<ModificationModel> Modifications { get; init; }
}

// Requests and responses
public record SaleTransactionItemCreateRequest {
    [DefaultValue(1)]
    public int ItemAmount { get; init; }
    public int SaleItemId { get; init; }
    public ModificationCreateRequest[] Modifications { get; init; } = [];
}
