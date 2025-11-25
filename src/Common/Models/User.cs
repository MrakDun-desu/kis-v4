using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record UserListModel {
    public required int Id { get; init; }
    // will probably need to add more things like username later, but that needs to be somehow
    // figured out with the authentication server
}

public record UserDetailModel {
    public required int Id { get; init; }
    public required AccountTransactionReadAllResponse PrestigeTransactions { get; init; }
}

// Requests and responses
public record UserReadAllRequest : PagedRequest;

public record UserReadAllResponse : PagedResponse<UserListModel>;

public record UserReadResponse : UserDetailModel;
