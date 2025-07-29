using System.Security.Claims;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class SaleTransactions {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("sale-transactions");
        group.MapGet(string.Empty, ReadAll);
        group.MapGet("self-cancellable", ReadSelfCancellable);
        group.MapPost(string.Empty, Create);
        group.MapPatch("{id:int}", Patch);
        group.MapGet("{id:int}", Read);
        group.MapPost("{id:int}/finish", Finish);
        group.MapDelete("{id:int}", Delete);
    }

    private static Results<Ok<Page<SaleTransactionListModel>>, ValidationProblem> ReadAll(
        ISaleTransactionService saleTransactionService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] DateTimeOffset? startDate,
        [FromQuery] DateTimeOffset? endDate,
        [FromQuery] bool? cancelled
    ) {
        return saleTransactionService.ReadAll(page, pageSize, startDate, endDate, cancelled)
            .Match<Results<Ok<Page<SaleTransactionListModel>>, ValidationProblem>>(
                static output => TypedResults.Ok(output),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static List<SaleTransactionListModel> ReadSelfCancellable(
        ISaleTransactionService saleTransactionService,
        ClaimsPrincipal claims) {
        return [.. saleTransactionService.ReadSelfCancellable(claims.Identity!.Name!)];
    }

    private static Results<Ok<SaleTransactionDetailModel>, ValidationProblem> Create(
        ISaleTransactionService saleTransactionService,
        SaleTransactionCreateModel createModel,
        ClaimsPrincipal claims) {
        return saleTransactionService.Create(createModel, claims.Identity!.Name!)
            .Match<Results<Ok<SaleTransactionDetailModel>, ValidationProblem>>(
                static output => TypedResults.Ok(output),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<SaleTransactionDetailModel>, NotFound, ValidationProblem> Patch(
            ISaleTransactionService saleTransactionService,
            int id,
            SaleTransactionCreateModel updateModel,
            ClaimsPrincipal claims
            ) {
        return saleTransactionService.Patch(id, updateModel, claims.Identity!.Name!)
            .Match<Results<Ok<SaleTransactionDetailModel>, NotFound, ValidationProblem>>(
                    static output => TypedResults.Ok(output),
                    static _ => TypedResults.NotFound(),
                    static errors => TypedResults.ValidationProblem(errors)
                    );
    }

    private static Results<Ok<SaleTransactionDetailModel>, NotFound, ValidationProblem> Finish(
            ISaleTransactionService saleTransactionService,
            int id,
            IEnumerable<CurrencyChangeCreateModel> currencyChanges,
            ClaimsPrincipal claims
            ) {
        return saleTransactionService.Finish(id, [.. currencyChanges])
            .Match<Results<Ok<SaleTransactionDetailModel>, NotFound, ValidationProblem>>(
                    static output => TypedResults.Ok(output),
                    static _ => TypedResults.NotFound(),
                    static errors => TypedResults.ValidationProblem(errors)
                    );
    }

    private static Results<Ok<SaleTransactionDetailModel>, NotFound> Read(
        ISaleTransactionService saleTransactionService,
        int id) {
        return saleTransactionService.Read(id)
            .Match<Results<Ok<SaleTransactionDetailModel>, NotFound>>(
                static output => TypedResults.Ok(output),
                static _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok<SaleTransactionDetailModel>, NotFound> Delete(
        ISaleTransactionService saleTransactionService,
        int id) {
        return saleTransactionService.Delete(id)
            .Match<Results<Ok<SaleTransactionDetailModel>, NotFound>>(
                static output => TypedResults.Ok(output),
                static _ => TypedResults.NotFound()
            );
    }

}
