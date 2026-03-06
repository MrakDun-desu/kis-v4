using System.Security.Claims;
using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class StoreItems {
    private const string ReadRouteName = "StoreItemsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("store-items", ReadAll)
            .WithName("StoreItemsReadAll")
            .AddValidation<StoreItemReadAllRequest>();
        routeBuilder.MapPost("store-items", Create)
            .WithName("StoreItemsCreate")
            .AddValidation<StoreItemCreateRequest>();
        routeBuilder.MapGet("store-items/{id:int}", Read)
            .WithName("StoreItemsRead")
            .WithName(ReadRouteName);
        routeBuilder.MapPut("store-items/{id:int}", Update)
            .WithName("StoreItemsUpdate")
            .AddValidation<StoreItemUpdateRequest>();
        routeBuilder.MapDelete("store-items/{id:int}", Delete)
            .AddValidation<StoreItemDeleteRequest>()
            .WithName("StoreItemsDelete");
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
        [AsParameters]
        StoreItemUpdateRequest req,
        StoreItemService service,
        IValidator<StoreItemUpdateRequest> validator,
        CancellationToken token = default
    ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var output = await service.UpdateAsync(req, token);
        return output switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val),
        };
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
        [AsParameters]
        StoreItemDeleteRequest req,
        StoreItemService service,
        CancellationToken token = default
    ) {
        return (await service.DeleteAsync(req, token))
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
