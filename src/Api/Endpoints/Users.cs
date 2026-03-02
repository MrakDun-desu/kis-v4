using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Users {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("users", ReadAll)
            .AddValidation<UserReadAllRequest>();
    }

    public static async Task<Results<Ok<UserReadAllResponse>, ValidationProblem>> ReadAll(
        [AsParameters] UserReadAllRequest req,
        UserService service,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public static async Task<Results<Ok<UserReadResponse>, NotFound>> Read(
        int id,
        UserService service,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }
}
