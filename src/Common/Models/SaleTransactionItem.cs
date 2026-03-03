using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KisV4.Common.Models;

// Base models
public record SaleTransactionItemModel {
    public required int LineNumber { get; init; }
    public required int Amount { get; init; }
    public required string SaleItemName { get; init; }
    public required IEnumerable<ModificationModel> Modifications { get; init; }
    public required decimal BasePrice { get; init; }
}

// Requests and responses
public record SaleTransactionItemCreateRequest {
    [DefaultValue(1)]
    public required int Amount { get; init; }
    public required int SaleItemId { get; init; }
    public ModificationCreateRequest[] Modifications { get; init; } = [];
}
