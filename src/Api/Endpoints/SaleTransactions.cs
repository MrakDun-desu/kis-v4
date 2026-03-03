using System.Security.Claims;
using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class SaleTransactions {
    private const string ReadRouteName = "SaleTransactionsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("sale-transactions", ReadAll)
            .AddValidation<SaleTransactionReadAllRequest>();
        routeBuilder.MapGet("sale-transactions/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("sale-transactions", Create)
            .AddValidation<SaleTransactionCreateRequest>();
        routeBuilder.MapPost("sale-transactions/check-price", CheckPrice)
            .AddValidation<SaleTransactionCheckPriceRequest>();
        routeBuilder.MapPost("sale-transactions/open", Open)
            .AddValidation<SaleTransactionOpenRequest>();
        routeBuilder.MapPatch("sale-transactions/{id:int}", Update)
            .RequireAuthorization<SaleTransactionUpdateRequest>()
            .AddValidation<SaleTransactionUpdateRequest>();
        routeBuilder.MapPost("sale-transactions/{id:int}/close", Close)
            .RequireAuthorization<SaleTransactionCloseRequest>()
            .AddValidation<SaleTransactionCloseRequest>();
        routeBuilder.MapDelete("sale-transactions/{id:int}", Delete)
            .RequireAuthorization<SaleTransactionDeleteRequest>()
            .WithAdminOverride();
    }

    public static async Task<Results<
        Ok<SaleTransactionReadAllResponse>,
        ValidationProblem
    >> ReadAll(
        [AsParameters] SaleTransactionReadAllRequest req,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        var output = await service.ReadAllAsync(req, claims.GetUserId(), token);
        return TypedResults.Ok(output);
    }

    public static async Task<Results<
        Ok<SaleTransactionDetailModel>,
        NotFound
    >> Read(
        int id,
        SaleTransactionService service,
        CancellationToken token = default
    ) {
        var output = await service.ReadAsync(id, token);
        return output switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<
        CreatedAtRoute<SaleTransactionDetailModel>,
        ValidationProblem
    >> Create(
        SaleTransactionCreateRequest req,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        var output = await service.CreateAsync(req, claims.GetUserId(), token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<
        Ok<SaleTransactionCheckPriceResponse>,
        ValidationProblem
    >> CheckPrice(
        SaleTransactionCheckPriceRequest req,
        SaleTransactionService service,
        CancellationToken token = default
    ) {
        var output = await service.CheckPriceAsync(req, token);
        return TypedResults.Ok(output);
    }

    public static async Task<Results<
        CreatedAtRoute<SaleTransactionDetailModel>,
        ValidationProblem
    >> Open(
        SaleTransactionOpenRequest req,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        var output = await service.OpenAsync(req, claims.GetUserId(), token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<
        Ok<SaleTransactionDetailModel>,
        NotFound,
        ValidationProblem,
        ForbidHttpResult
    >> Update(
        [AsParameters]
        SaleTransactionUpdateRequest req,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        var output = await service.UpdateAsync(req, claims.GetUserId(), token);
        return output switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<
        Ok<SaleTransactionDetailModel>,
        NotFound,
        ValidationProblem,
        ForbidHttpResult
    >> Close(
        [AsParameters]
        SaleTransactionCloseRequest req,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        var output = await service.CloseAsync(req, claims.GetUserId(), token);
        return output switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<
        NoContent,
        NotFound,
        ForbidHttpResult
    >> Delete(
        [AsParameters]
        SaleTransactionDeleteRequest req,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        return await service.DeleteAsync(req, claims.GetUserId(), token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
