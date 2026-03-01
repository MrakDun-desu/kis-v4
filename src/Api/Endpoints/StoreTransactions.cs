using System.Security.Claims;
using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.Api.Endpoints;

public static class StoreTransactions {
    private const string ReadRouteName = "StoreTransactionsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("store-transactions", ReadAll);
        routeBuilder.MapGet("store-transactions/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("store-transactions", Create);
        routeBuilder.MapDelete("store-transactions/{id:int}", Delete);
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
        IValidator<StoreTransactionCreateRequest> validator,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        var userId = claims.GetUserId();
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var output = await service.CreateAsync(req, userId, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<NoContent, NotFound, ValidationProblem>> Delete(
        int id,
        [FromQuery] bool? updateCosts,
        // IValidator<StoreTransactionDeleteCommand> validator,
        ClaimsPrincipal claims,
        StoreTransactionService service,
        CancellationToken token = default
    ) {
        var userId = claims.GetUserId();
        var command = new StoreTransactionDeleteCommand(
            Id: id,
            UserId: userId,
            UpdateCosts: updateCosts
        );
        // var validationResult = await validator.ValidateAsync(command, token);
        // if (!validationResult.IsValid) {
        //     return TypedResults.ValidationProblem(validationResult.ToDictionary());
        // }

        return await service.DeleteAsync(command, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
