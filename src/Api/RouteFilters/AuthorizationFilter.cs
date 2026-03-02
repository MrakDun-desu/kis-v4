using KisV4.BL.EF.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace KisV4.Api.RouteFilters;

public class AuthorizationFilter<T>(
    IAuthorizationService authService,
    IEnumerable<IAuthorizationRequirement> requirements
) : IEndpointFilter
where T : class {
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    ) {
        var typeName = typeof(T).Name;
        var user = context.HttpContext.User;

        var req = context.Arguments.OfType<T>().FirstOrDefault() ??
            throw new InvalidOperationException($"""
                Tried authorizing user for a request of type {typeName} in an endpoint
                without an argument of the specified type.
            """);

        var result = await authService.AuthorizeAsync(user, req, requirements);

        if (!result.Succeeded) {
            return TypedResults.Forbid();
        }


        return await next(context);
    }
}
