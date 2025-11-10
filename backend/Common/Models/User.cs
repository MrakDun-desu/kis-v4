using KisV4.Common.ModelWrappers;

namespace KisV4.Common.Models;

// Base models
public record UserModel {
    public required int Id { get; init; }
    public required string Username { get; init; }
}

// Requests and responses
public record UserReadAllRequest : PagedRequest {
    public string? Username { get; init; }
}

public record UserReadAllResponse : PagedResponse<UserModel>;
