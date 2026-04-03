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
    }

    public static async Task<Results<Ok<AccountTransactionReadAllResponse>, ValidationProblem>> ReadAll(
            AccountTransactionService service,
            [AsParameters] AccountTransactionReadAllRequest req,
            CancellationToken token = default
            ) {
        return TypedResults.Ok(await service.ReadAllAsync(req, token));
    }
}
