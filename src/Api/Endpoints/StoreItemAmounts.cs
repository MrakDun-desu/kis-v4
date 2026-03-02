using FluentValidation;
using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class StoreItemAmounts {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("store-item-amounts", ReadAll)
            .AddValidation<StoreItemAmountReadAllRequest>();
    }

    public static async Task<Results<Ok<StoreItemAmountReadAllResponse>, ValidationProblem>> ReadAll(
            [AsParameters] StoreItemAmountReadAllRequest req,
            StoreItemAmountService service,
            CancellationToken token = default
            ) {
        return TypedResults.Ok(await service.ReadAllAsync(req, token));
    }
}
