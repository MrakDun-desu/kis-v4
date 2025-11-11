using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Users {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("users", ReadAll);
    }

    public static UserReadAllResponse ReadAll([AsParameters] UserReadAllRequest req) {
        throw new NotImplementedException();
    }
}
