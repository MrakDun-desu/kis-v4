using KisV4.BL.EF.Authorization.Flags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace KisV4.BL.EF.Authorization.Handlers;

public class AdminOverrideHandler(
    IHttpContextAccessor contextAccessor
) : IAuthorizationHandler {
    public Task HandleAsync(AuthorizationHandlerContext context) {
        // if the user isn't admin, no need to check any further
        if (!context.User.IsInRole("Admin")) {
            return Task.CompletedTask;
        }

        // if the user is admin, also check if this override has been allowed
        var endpoint = contextAccessor.HttpContext?.GetEndpoint();
        var allowOverride = endpoint?.Metadata.GetMetadata<AllowAdminOverride>() is not null;
        if (!allowOverride) {
            return Task.CompletedTask;
        }

        // if user is admin and it's allowed to override requirements, mark all requirements as succeeded
        foreach (var requirement in context.PendingRequirements) {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
