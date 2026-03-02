using System.Security.Claims;
using FluentValidation;
using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Authorization.Requirements;
using KisV4.BL.EF.Services;
using KisV4.Common;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class StoreTransactions {
    private const string ReadRouteName = "StoreTransactionsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("store-transactions", ReadAll)
            .AddValidation<StoreTransactionReadAllRequest>();
        routeBuilder.MapGet("store-transactions/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("store-transactions", Create)
            .AddValidation<StoreTransactionCreateRequest>();
        routeBuilder.MapDelete("store-transactions/{id:int}", Delete)
            .RequireAuthorization<StoreTransactionDeleteRequest>()
            .WithAdminOverride();
    }

    public static async Task<Results<Ok<StoreTransactionReadAllResponse>, ValidationProblem>> ReadAll(
        [AsParameters] StoreTransactionReadAllRequest req,
        StoreTransactionService service,
        IValidator<StoreTransactionReadAllRequest> validator,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var userId = claims.GetUserId();
        return TypedResults.Ok(await service.ReadAllAsync(req, userId, token));
    }

    public static async Task<Results<Ok<StoreTransactionReadResponse>, NotFound>> Read(
        int id,
        StoreTransactionService service,
        CancellationToken token = default
    ) {
        return await service.ReadAsync(id, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<CreatedAtRoute<StoreTransactionCreateResponse>, ValidationProblem>> Create(
        StoreTransactionCreateRequest req,
        StoreTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        var userId = claims.GetUserId();
        var output = await service.CreateAsync(req, userId, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<NoContent, NotFound, ValidationProblem>> Delete(
        [AsParameters]
        StoreTransactionDeleteRequest req,
        ClaimsPrincipal claims,
        StoreTransactionService service,
        CancellationToken token = default
    ) {
        var userId = claims.GetUserId();
        return await service.DeleteAsync(req, userId, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
