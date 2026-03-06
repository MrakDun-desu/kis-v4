using FluentValidation;
using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class SaleItems {
    private const string ReadRouteName = "SaleItemsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("sale-items", ReadAll)
            .WithName("SaleItemsReadAll")
            .AddValidation<SaleItemReadAllRequest>();
        routeBuilder.MapPost("sale-items", Create)
            .WithName("SaleItemsCreate")
            .AddValidation<SaleItemCreateRequest>();
        routeBuilder.MapGet("sale-items/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPut("sale-items/{id:int}", Update)
            .WithName("SaleItemsUpdate")
            .AddValidation<SaleItemUpdateRequest>();
        routeBuilder.MapDelete("sale-items/{id:int}", Delete)
            .WithName("SaleItemsDelete");
    }

    public static async Task<Results<Ok<SaleItemReadAllResponse>, ValidationProblem>> ReadAll(
        [AsParameters] SaleItemReadAllRequest req,
        SaleItemService service,
        CancellationToken token = default
    ) {
        var data = await service.ReadAllAsync(req, token);
        return TypedResults.Ok(data);
    }

    public static async Task<Results<Ok<SaleItemReadResponse>, NotFound>> Read(
        int id,
        SaleItemService service,
        CancellationToken token = default
    ) {
        return await service.ReadAsync(id, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<CreatedAtRoute<SaleItemCreateResponse>, ValidationProblem>> Create(
        SaleItemCreateRequest req,
        SaleItemService service,
        CancellationToken token = default
    ) {
        var output = await service.CreateAsync(req, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<Ok<SaleItemUpdateResponse>, NotFound, ValidationProblem>> Update(
        [AsParameters]
        SaleItemUpdateRequest req,
        SaleItemService service,
        CancellationToken token = default
    ) {
        return await service.UpdateAsync(req, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
        int id,
        SaleItemService service,
        CancellationToken token = default
    ) {
        return await service.DeleteAsync(id, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
