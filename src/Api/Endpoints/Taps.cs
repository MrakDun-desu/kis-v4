using System.Security.Claims;
using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Taps {
    private const string ReadRouteName = "TapsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("taps", ReadAll)
            .WithName("TapsReadAll");
        routeBuilder.MapGet("taps/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("taps", Create)
            .WithName("TapsCreate")
            .AddValidation<TapCreateRequest>();
        routeBuilder.MapPut("taps/{id:int}", Update)
            .WithName("TapsUpdate")
            .AddValidation<TapUpdateRequest>();
        routeBuilder.MapDelete("taps/{id:int}", Delete)
            .WithName("TapsDelete");
    }

    public static async Task<TapReadAllResponse> ReadAll(
        TapService service,
        CancellationToken token = default
    ) {
        return await service.ReadAllAsync(token);
    }

    public static async Task<Results<Ok<TapReadResponse>, NotFound>> Read(
        [AsParameters]
        TapReadRequest req,
        TapService service,
        CancellationToken token = default
    ) {
        return await service.ReadAsync(req, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<CreatedAtRoute<TapCreateResponse>, ValidationProblem>> Create(
        TapCreateRequest req,
        TapService service,
        CancellationToken token = default
    ) {
        var output = await service.CreateAsync(req, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<Ok<TapUpdateResponse>, NotFound, ValidationProblem>> Update(
        [AsParameters]
        TapUpdateRequest req,
        ClaimsPrincipal claims,
        TapService service,
        CancellationToken token = default
    ) {
        var output = await service.UpdateAsync(req, claims.Identity!.Name!, token);
        return TypedResults.Ok(output);
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
        int id,
        TapService service,
        CancellationToken token = default
    ) {
        return await service.DeleteAsync(id, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
