using System.Security.Claims;
using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.Api.Endpoints;

public static class SaleTransactions {
    private const string ReadRouteName = "SaleTransactionsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("sale-transactions", ReadAll);
        routeBuilder.MapGet("sale-transactions/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("sale-transactions", Create);
        routeBuilder.MapPost("sale-transactions/check-price", CheckPrice);
        routeBuilder.MapPost("sale-transactions/open", Open);
        routeBuilder.MapPatch("sale-transactions/{id:int}", Update);
        routeBuilder.MapPost("sale-transactions/{id:int}/close", Close);
        routeBuilder.MapDelete("sale-transactions/{id:int}", Delete);
    }

    public static async Task<Results<Ok<SaleTransactionReadAllResponse>, ValidationProblem>> ReadAll(
        [AsParameters] SaleTransactionReadAllRequest req,
        // IValidator<SaleTransactionReadAllRequest> validator,
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
        // IValidator<SaleTransactionCreateRequest> validator,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public static async Task<Results<Ok<SaleTransactionCheckPriceResponse>, ValidationProblem>> CheckPrice(
        SaleTransactionCheckPriceRequest req,
        // IValidator<SaleTransactionCheckPriceRequest> validator,
        SaleTransactionService service,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public static async Task<Results<CreatedAtRoute<SaleTransactionOpenResponse>, ValidationProblem>> Open(
        SaleTransactionOpenRequest req,
        // IValidator<SaleTransactionOpenRequest> validator,
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
        int id,
        SaleTransactionUpdateRequest req,
        // IValidator<SaleTransactionUpdateRequest> validator,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public static async Task<Results<Ok<SaleTransactionCloseResponse>, NotFound, ValidationProblem>> Close(
        int id,
        SaleTransactionCloseRequest req,
        // IValidator<SaleTransactionCloseCommand> validator,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
        int id,
        // IValidator<SaleTransactionDeleteCommand> validator,
        SaleTransactionService service,
        ClaimsPrincipal claims,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }
}
