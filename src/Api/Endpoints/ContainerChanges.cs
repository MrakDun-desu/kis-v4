using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class ContainerChanges {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("container-changes", ReadAll);
        routeBuilder.MapPost("container-changes", Create);
    }

    public static Results<Ok<ContainerChangeReadAllResponse>, NotFound> ReadAll([AsParameters] ContainerChangeReadAllRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Created<ContainerChangeCreateResponse>, ValidationProblem> Create(ContainerChangeCreateRequest req) {
        throw new NotImplementedException();
    }
}
