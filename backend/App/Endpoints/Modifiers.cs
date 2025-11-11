using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Modifiers {
    private const string ReadRouteName = "ModifiersRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("modifiers", ReadAll);
        routeBuilder.MapGet("modifers/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("modifiers", Create);
        routeBuilder.MapPut("modifiers/{id:int}", Update);
        routeBuilder.MapDelete("modifiers/{id:int}", Delete);
    }

    public static ModifierReadAllResponse ReadAll([AsParameters] ModifierReadAllRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<ModifierReadResponse>, NotFound> Read(int id) {
        throw new NotImplementedException();
    }

    public static Results<CreatedAtRoute<ModifierCreateResponse>, ValidationProblem> Create(ModifierCreateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<ModifierUpdateResponse>, NotFound, ValidationProblem> Update(int id, ModifierUpdateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
