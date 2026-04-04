using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models

// used to show the users in the admin UI
public record UserListModel {
    public required string Id { get; init; }
    public required string? Nick { get; init; }
}

// used to show user detail in the admin UI
public record UserDetailModel {
    public required string Id { get; init; }
    public required string? Nick { get; init; }
    public required AccountTransactionReadAllResponse AccountTransactions { get; init; }
}

// used to show prestige for users
public record UserPrestigeModel {
    public required string Id { get; init; }
    public required string? Nick { get; init; }
    public required decimal TotalPrestige { get; init; }
    public required decimal DailyPrestige { get; init; }
    public required decimal WeeklyPrestige { get; init; }
    public required decimal MonthlyPrestige { get; init; }
}

// Requests and responses
public record UserReadAllRequest : PagedRequest;

public record UserReadAllResponse : PagedResponse<UserListModel>;

public record UserReadResponse : UserDetailModel;
