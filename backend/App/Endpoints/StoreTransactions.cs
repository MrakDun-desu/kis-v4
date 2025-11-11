using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class StoreTransactions {
    private const string ReadRouteName = "StoreTransactionsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("store-transactions", ReadAll);
        routeBuilder.MapGet("store-transactions/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("store-transactions", Create);
        routeBuilder.MapDelete("store-transactions/{id:int}", Delete);
    }

    public static StoreTransactionReadAllResponse ReadAll([AsParameters] StoreTransactionReadAllRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<StoreTransactionReadResponse>, NotFound> Read(int id) {
        throw new NotImplementedException();
    }

    public static Results<CreatedAtRoute<StoreTransactionCreateResponse>, ValidationProblem> Create(StoreTransactionCreateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
