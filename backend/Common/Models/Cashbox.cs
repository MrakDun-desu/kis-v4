using KisV4.Common.ModelWrappers;

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
    public required StockTakingReadAllResponse StockTakings { get; init; }
}

// Requests and responses
public record CashBoxReadAllResponse : CollectionResponse<CashBoxListModel>;

public record CashBoxCreateRequest {
    public required string Name { get; init; }
}

public record CashBoxCreateResponse : CashBoxListModel;

public record CashBoxUpdateRequest {
    public required string Name { get; init; }
}

public record CashBoxUpdateResponse : CashBoxListModel;

public record CashBoxReadResponse : CashBoxDetailModel;


