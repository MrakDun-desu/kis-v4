using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class AccountTransactions {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("account-transactions", ReadAll)
            .WithName("AccountTransactionsReadAll")
            .AddValidation<AccountTransactionReadAllRequest>();
        routeBuilder.MapPost("account-transactions", Create)
            .WithName("AccountTransactionsCreate")
            .AddValidation<AccountTransactionCreateRequest>();
    }

    public static async Task<Results<Ok<AccountTransactionReadAllResponse>, ValidationProblem>> ReadAll(
            AccountTransactionService service,
            [AsParameters] AccountTransactionReadAllRequest req,
            CancellationToken token = default
            ) {
        return TypedResults.Ok(await service.ReadAllAsync(req, token));
    }

    public static async Task<Results<Ok<AccountTransactionCreateResponse>, ValidationProblem>> Create(
            AccountTransactionService service,
            AccountTransactionCreateRequest req,
            CancellationToken token = default
            ) {
        return TypedResults.Ok(await service.CreateAsync(req, token));
    }
}
