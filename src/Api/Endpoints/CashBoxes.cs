using FluentValidation;
using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class CashBoxes {
    private const string ReadRouteName = "CashBoxesRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("cashboxes", ReadAll);
        routeBuilder.MapPost("cashboxes", Create)
            .AddValidation<CashBoxCreateRequest>();
        routeBuilder.MapGet("cashboxes/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPut("cashboxes/{id:int}", Update)
            .AddValidation<CashBoxUpdateRequest>();
        routeBuilder.MapDelete("cashboxes/{id:int}", Delete);
    }

    public static async Task<CashBoxReadAllResponse> ReadAll(
            CashBoxService service,
            CancellationToken token = default
            ) {
        return await service.ReadAllAsync(token);
    }

    public static async Task<Results<CreatedAtRoute<CashBoxCreateResponse>, ValidationProblem>> Create(
            CashBoxService service,
            [AsParameters]
            CashBoxCreateRequest req,
            CancellationToken token = default
            ) {
        var output = await service.CreateAsync(req, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<Ok<CashBoxReadResponse>, NotFound>> Read(
        [AsParameters]
        CashBoxReadRequest req,
        CashBoxService service,
            CancellationToken token = default
            ) {
        return await service.ReadAsync(req, token) switch {
            null => TypedResults.NotFound(),
            var response => TypedResults.Ok(response)
        };
    }

    public static async Task<Results<Ok<CashBoxUpdateResponse>, NotFound, ValidationProblem>> Update(
            CashBoxService service,
            [AsParameters]
            CashBoxUpdateRequest req,
            CancellationToken token = default
            ) {
        return await service.UpdateAsync(req, token) switch {
            null => TypedResults.NotFound(),
            var response => TypedResults.Ok(response)
        };
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
            CashBoxService service,
            [AsParameters]
            CashBoxDeleteRequest req,
            CancellationToken token = default
            ) {
        return await service.DeleteAsync(req, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
