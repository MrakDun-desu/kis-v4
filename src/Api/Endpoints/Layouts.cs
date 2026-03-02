using FluentValidation;
using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Layouts {
    private const string ReadRouteName = "LayoutsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("layouts", ReadAll);
        routeBuilder.MapGet("layouts/{id:int}", Read)
            .AddValidation<LayoutReadRequest>()
            .WithName(ReadRouteName);
        routeBuilder.MapGet("layouts/top-level", ReadTopLevel)
            .AddValidation<LayoutReadTopLevelRequest>();
        routeBuilder.MapPost("layouts", Create)
            .AddValidation<LayoutCreateRequest>();
        routeBuilder.MapPut("layouts/{id:int}", Update)
            .AddValidation<LayoutUpdateRequest>();
        routeBuilder.MapDelete("layouts/{id:int}", Delete);
    }

    private static async Task<LayoutReadAllResponse> ReadAll(
        [AsParameters] LayoutReadAllRequest req,
        LayoutService service,
        CancellationToken token = default
    ) {
        return await service.ReadAllAsync(req, token);
    }

    private static async Task<Results<Ok<LayoutReadResponse>, ValidationProblem>> Read(
        [AsParameters]
        LayoutReadRequest req,
        LayoutService service,
        CancellationToken token = default
    ) {
        return TypedResults.Ok(await service.ReadAsync(req, token));
    }

    private static async Task<Results<Ok<LayoutReadResponse>, NotFound, ValidationProblem>> ReadTopLevel(
        [AsParameters]
        LayoutReadTopLevelRequest req,
        LayoutService service,
        CancellationToken token = default
    ) {
        return await service.ReadTopLevelAsync(req, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    private static async Task<Results<CreatedAtRoute<LayoutCreateResponse>, ValidationProblem>> Create(
        [AsParameters]
        LayoutCreateRequest req,
        LayoutService service,
        CancellationToken token = default
    ) {
        var output = await service.CreateAsync(req, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    private static async Task<Results<Ok<LayoutUpdateResponse>, NotFound, ValidationProblem>> Update(
        [AsParameters]
        LayoutUpdateRequest req,
        LayoutService service,
        CancellationToken token = default
    ) {
        var output = await service.UpdateAsync(req, token);
        return TypedResults.Ok(output);
    }

    private static async Task<Results<NoContent, NotFound>> Delete(
        int id,
        LayoutService service,
        CancellationToken token = default
    ) {
        return await service.DeleteAsync(id, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
