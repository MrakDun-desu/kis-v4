using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Layouts {
    private const string ReadRouteName = "LayoutsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("layouts", ReadAll);
        routeBuilder.MapGet("layouts/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("layouts", Create);
        routeBuilder.MapPut("layouts/{id:int}", Update);
        routeBuilder.MapDelete("layouts/{id:int}", Delete);
    }

    public static LayoutReadAllResponse ReadAll([AsParameters] LayoutReadAllRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<LayoutReadResponse>, ValidationProblem> Read(int id) {
        throw new NotImplementedException();
    }

    public static Results<CreatedAtRoute<LayoutCreateResponse>, ValidationProblem> Create(LayoutCreateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<LayoutUpdateResponse>, NotFound, ValidationProblem> Update(int id, LayoutUpdateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
