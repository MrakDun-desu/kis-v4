using System.ComponentModel;
using KisV4.Common.ModelWrappers;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.Common.Models;

// Base models
public record CashBoxListModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
}

public record CashBoxDetailModel {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required AccountTransactionReadAllResponse SalesTransactions { get; init; }
    public required AccountTransactionReadAllResponse DonationsTransactions { get; init; }
    public required IEnumerable<DateTimeOffset> StockTakings { get; init; }
}

// Requests and responses
public record CashBoxReadAllResponse : CollectionResponse<CashBoxListModel>;

public record CashBoxReadRequest {
    [FromRoute]
    public required int Id { get; init; }
};

public record CashBoxCreateRequest {
    [DefaultValue("Kasa Kachna")]
    public required string Name { get; init; }
}

public record CashBoxCreateResponse : CashBoxListModel;

public record CashBoxUpdateRequestModel {
    [DefaultValue("Kasa Kachna")]
    public required string Name { get; init; }
}

public record CashBoxUpdateRequest {
    [FromRoute]
    public required int Id { get; init; }
    [FromBody]
    public required CashBoxUpdateRequestModel Model { get; init; }
};

public record CashBoxUpdateResponse : CashBoxListModel;

public record CashBoxReadResponse : CashBoxDetailModel;

public record CashBoxDeleteRequest {
    [FromRoute]
    public required int Id { get; init; }
};
