using System.Security.Claims;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class StoreTransactions {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("store-transactions");
        group.MapGet(string.Empty, ReadAll);
        group.MapGet("self-cancellable", ReadSelfCancellable);
        group.MapPost(string.Empty, Create);
        group.MapGet("{id:int}", Read);
        group.MapDelete("{id:int}", Delete);
    }

    private static Results<Ok<Page<StoreTransactionListModel>>, ValidationProblem> ReadAll(
        IStoreTransactionService storeTransactionService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] DateTimeOffset? startDate,
        [FromQuery] DateTimeOffset? endDate,
        [FromQuery] bool? cancelled
    ) {
        return storeTransactionService.ReadAll(page, pageSize, startDate, endDate, cancelled)
            .Match<Results<Ok<Page<StoreTransactionListModel>>, ValidationProblem>>(
                output => TypedResults.Ok(output),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static IEnumerable<StoreTransactionListModel> ReadSelfCancellable(
        IStoreTransactionService storeTransactionService,
        ClaimsPrincipal claims) {
        return storeTransactionService.ReadSelfCancellable(claims.Identity!.Name!);
    }

    private static Results<Ok<StoreTransactionDetailModel>, ValidationProblem> Create(
        IStoreTransactionService storeTransactionService,
        StoreTransactionCreateModel createModel,
        ClaimsPrincipal claims) {
        return storeTransactionService.Create(createModel, claims.Identity!.Name!)
            .Match<Results<Ok<StoreTransactionDetailModel>, ValidationProblem>>(
                output => TypedResults.Ok(output),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<StoreTransactionDetailModel>, NotFound> Read(
        IStoreTransactionService storeTransactionService,
        int id) {
        return storeTransactionService.Read(id)
            .Match<Results<Ok<StoreTransactionDetailModel>, NotFound>>(
                output => TypedResults.Ok(output),
                _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok<StoreTransactionDetailModel>, NotFound> Delete(
        IStoreTransactionService storeTransactionService,
        int id) {
        return storeTransactionService.Delete(id)
            .Match<Results<Ok<StoreTransactionDetailModel>, NotFound>>(
                output => TypedResults.Ok(output),
                _ => TypedResults.NotFound()
            );
    }
}
