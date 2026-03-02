using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Authorization.Flags;
using KisV4.BL.EF.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace KisV4.Api;

public static class RouteBuilderExtensions {
    public static RouteHandlerBuilder AddValidation<T>(
        this RouteHandlerBuilder builder
    ) {
        return builder.AddEndpointFilter<ValidationFilter<T>>();
    }

    /// <summary>
    /// Authorizes the user with the specified requirements for the given type.
    /// The resource type needs to be one of the parameters of the given endpoint.
    /// Adds a PassThroughRequirement by default, which means that the requirements
    /// are not important in that case, just that the request should be authorized.
    /// </summary
    public static RouteHandlerBuilder RequireAuthorization<T>(
        this RouteHandlerBuilder builder,
        params List<IAuthorizationRequirement> requirements
    ) where T : class {
        if (requirements.Count == 0) {
            requirements.Add(new PassThroughRequirement());
        }
        return builder.AddEndpointFilter(async (context, next) => {
            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var filter = new AuthorizationFilter<T>(authService, requirements);
            return await filter.InvokeAsync(context, next);
        });
    }

    public static RouteHandlerBuilder WithAdminOverride(
        this RouteHandlerBuilder builder
    ) {
        return builder.WithMetadata(new AllowAdminOverride());
    }
}
