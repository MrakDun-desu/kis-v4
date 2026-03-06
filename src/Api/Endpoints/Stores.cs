using FluentValidation;
using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Stores {
    private const string ReadRouteName = "StoresRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("stores", ReadAll)
            .WithName("StoresReadAll");
        routeBuilder.MapGet("stores/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("stores", Create)
            .WithName("StoresCreate")
            .AddValidation<StoreCreateRequest>();
        routeBuilder.MapPut("stores/{id:int}", Update)
            .WithName("StoresUpdate")
            .AddValidation<StoreUpdateRequest>();
        routeBuilder.MapDelete("stores/{id:int}", Delete)
            .WithName("StoresDelete");
    }

    public static async Task<StoreReadAllResponse> ReadAll(
            StoreService service,
            CancellationToken token = default
            ) {
        return await service.ReadAllAsync(token);
    }

    public static async Task<Results<Ok<StoreReadResponse>, NotFound>> Read(
            int id,
            StoreService service,
            CancellationToken token = default
            ) {
        return await service.ReadAsync(id, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<CreatedAtRoute<StoreCreateResponse>, ValidationProblem>> Create(
            StoreCreateRequest req,
            StoreService service,
            CancellationToken token = default
            ) {
        var output = await service.CreateAsync(req, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<Ok<StoreUpdateResponse>, NotFound, ValidationProblem>> Update(
            int id,
            StoreUpdateRequest req,
            StoreService service,
            CancellationToken token = default
            ) {
        return await service.UpdateAsync(id, req, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
            int id,
            StoreService service,
            CancellationToken token = default
            ) {
        return await service.DeleteAsync(id, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
