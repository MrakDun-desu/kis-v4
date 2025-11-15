using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Containers {
    private const string ReadRouteName = "ContainersRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("containers", ReadAll);
        routeBuilder.MapGet("containers/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("containers", Create);
        routeBuilder.MapPut("containers/{id:int}", Update);
        routeBuilder.MapDelete("containers/{id:int}", Delete);
    }

    public static ContainerReadAllResponse ReadAll(
            [AsParameters] ContainerReadAllRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<ContainerReadResponse>, NotFound> Read(int id) {
        throw new NotImplementedException();
    }

    public static Results<CreatedAtRoute<ContainerCreateResponse>, ValidationProblem> Create(ContainerCreateRequest req) {
        throw new NotImplementedException();
    }

    public static ContainerUpdateResponse Update(int id, ContainerUpdateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
