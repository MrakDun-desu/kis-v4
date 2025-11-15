using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Pipes {
    private const string ReadRouteName = "PipesRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("pipes", ReadAll);
        routeBuilder.MapGet("pipes/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("pipes", Create);
        routeBuilder.MapPut("pipes/{id:int}", Update);
        routeBuilder.MapDelete("pipes/{id:int}", Delete);
    }

    public static PipeReadAllResponse ReadAll() {
        throw new NotImplementedException();
    }

    public static Results<Ok<PipeReadResponse>, NotFound> Read(int id) {
        throw new NotImplementedException();
    }

    public static CreatedAtRoute<PipeCreateResponse> Create(PipeCreateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<PipeUpdateResponse>, NotFound> Update(int id, PipeUpdateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
