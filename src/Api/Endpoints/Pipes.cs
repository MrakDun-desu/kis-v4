using FluentValidation;
using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.BL.EF.Validation;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Pipes {
    private const string ReadRouteName = "PipesRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("pipes", ReadAll)
            .WithName("PipesReadAll");
        routeBuilder.MapGet("pipes/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("pipes", Create)
            .WithName("PipesCreate")
            .AddValidation<PipeCreateRequest>();
        routeBuilder.MapPut("pipes/{id:int}", Update)
            .WithName("PipesUpdate")
            .AddValidation<PipeUpdateRequest>();
        routeBuilder.MapDelete("pipes/{id:int}", Delete)
            .WithName("PipesDelete");
    }

    public static async Task<PipeReadAllResponse> ReadAll(
        PipeService service,
        CancellationToken token = default
    ) {
        return await service.ReadAllAsync(token);
    }

    public static async Task<Results<Ok<PipeReadResponse>, NotFound>> Read(
        int id,
        PipeService service,
        CancellationToken token = default
    ) {
        return await service.ReadAsync(id, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<CreatedAtRoute<PipeCreateResponse>, ValidationProblem>> Create(
        PipeCreateRequest req,
        PipeService service,
        CancellationToken token = default
    ) {
        var output = await service.CreateAsync(req, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<Ok<PipeUpdateResponse>, NotFound, ValidationProblem>> Update(
        [AsParameters]
        PipeUpdateRequest req,
        PipeService service,
        CancellationToken token = default
    ) {
        var output = await service.UpdateAsync(req, token);
        return TypedResults.Ok(output);
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
        int id,
        PipeService service,
        CancellationToken token = default
    ) {
        return await service.DeleteAsync(id, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
