using KisV4.Common.Models;
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
        routeBuilder.MapGet("sale-transactions/check-price", CheckPrice);
        routeBuilder.MapPost("sale-transactions/open", Open);
        routeBuilder.MapPatch("sale-transactions/{id:int}", Update);
        routeBuilder.MapPost("sale-transactions/{id:int}/close", Close);
        routeBuilder.MapDelete("sale-transactions/{id:int}", Delete);
    }

    public static SaleTransactionReadAllResponse ReadAll([AsParameters] SaleTransactionReadAllRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<SaleTransactionReadResponse>, NotFound> Read(int id) {
        throw new NotImplementedException();
    }

    public static Results<CreatedAtRoute<SaleTransactionCreateResponse>, ValidationProblem> Create(SaleTransactionCreateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<SaleTransactionCheckPriceResponse>, ValidationProblem> CheckPrice([FromBody] SaleTransactionCheckPriceRequest req) {
        throw new NotImplementedException();
    }

    public static Results<CreatedAtRoute<SaleTransactionOpenResponse>, ValidationProblem> Open(SaleTransactionOpenRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<SaleTransactionUpdateResponse>, NotFound, ValidationProblem> Update(int id, SaleTransactionUpdateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<SaleTransactionCloseResponse>, NotFound, ValidationProblem> Close(int id, SaleTransactionCloseRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
