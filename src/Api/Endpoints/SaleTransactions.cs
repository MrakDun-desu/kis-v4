using System.Security.Claims;
using FluentValidation;
using KisV4.BL.EF.Services;
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
            .RequireAuthorization<SaleTransactionCreateRequest>()
            .AddValidation<SaleTransactionUpdateRequest>();
        routeBuilder.MapPost("sale-transactions/{id:int}/close", Close)
            .RequireAuthorization<SaleTransactionCloseRequest>()
            .AddValidation<SaleTransactionCloseRequest>();
        routeBuilder.MapDelete("sale-transactions/{id:int}", Delete)
            .RequireAuthorization<SaleTransactionDeleteRequest>()
            .WithAdminOverride();
    }

    public static async Task<Results<Ok<SaleTransactionReadAllResponse>, ValidationProblem>> ReadAll(
        [AsParameters] SaleTransactionReadAllRequest req,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public static async Task<Results<Ok<SaleTransactionReadResponse>, NotFound>> Read(
        int id,
        SaleTransactionService service,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public static async Task<Results<CreatedAtRoute<SaleTransactionCreateResponse>, ValidationProblem>> Create(
        SaleTransactionCreateRequest req,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public static async Task<Results<Ok<SaleTransactionCheckPriceResponse>, ValidationProblem>> CheckPrice(
        SaleTransactionCheckPriceRequest req,
        SaleTransactionService service,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public static async Task<Results<CreatedAtRoute<SaleTransactionOpenResponse>, ValidationProblem>> Open(
        SaleTransactionOpenRequest req,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public static async Task<Results<
        Ok<SaleTransactionUpdateResponse>,
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
        throw new NotImplementedException();
    }

    public static async Task<Results<Ok<SaleTransactionCloseResponse>, NotFound, ValidationProblem>> Close(
        [AsParameters]
        SaleTransactionCloseRequest req,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
        int id,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }
}
