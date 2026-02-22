using System.Security.Claims;
using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class StoreItems {
    private const string ReadRouteName = "StoreItemsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("store-items", ReadAll);
        routeBuilder.MapPost("store-items", Create);
        routeBuilder.MapGet("store-items/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPut("store-items/{id:int}", Update);
        routeBuilder.MapDelete("store-items/{id:int}", Delete);
    }

    public static async Task<Results<Ok<StoreItemReadAllResponse>, ValidationProblem>> ReadAll(
            [AsParameters] StoreItemReadAllRequest req,
            StoreItemService service,
            IValidator<StoreItemReadAllRequest> validator,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.ReadAllAsync(req, token));
    }

    public static async Task<Results<Ok<StoreItemReadResponse>, NotFound>> Read(
            int id,
            StoreItemService service,
            CancellationToken token = default
            ) {
        return (await service.ReadAsync(id, token)) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<CreatedAtRoute<StoreItemCreateResponse>, ValidationProblem>> Create(
            StoreItemCreateRequest req,
            StoreItemService service,
            IValidator<StoreItemCreateRequest> validator,
            ClaimsPrincipal claims,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var userId = claims.GetUserId();
        var data = await service.CreateAsync(req, userId, token);

        return TypedResults.CreatedAtRoute(data, ReadRouteName, new { id = data.Id });
    }

    public static async Task<Results<Ok<StoreItemUpdateResponse>, NotFound, ValidationProblem>> Update(
            int id,
            StoreItemUpdateRequest req,
            StoreItemService service,
            IValidator<StoreItemUpdateRequest> validator,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var output = await service.UpdateAsync(id, req, token);
        return output switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(output),
        };
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
            int id,
            StoreItemService service,
            CancellationToken token = default
            ) {
        return (await service.DeleteAsync(id, token))
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
