using KisV4.App.Auth;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class StoreItemAmounts {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("store-item-amounts");
        group.MapGet(string.Empty, ReadAll);
    }

    private static Results<Ok<Page<SaleItemAmountListModel>>, ValidationProblem> ReadAll(
        ISaleItemAmountService saleItemAmountService,
        int storeId,
        int? page,
        int? pageSize,
        int? categoryId
    ) {
        return saleItemAmountService.ReadAll(storeId, page, pageSize, categoryId)
            .Match<Results<Ok<Page<SaleItemAmountListModel>>, ValidationProblem>>(
                output => TypedResults.Ok(output),
                errors => TypedResults.ValidationProblem(errors)
            );
    }
}
