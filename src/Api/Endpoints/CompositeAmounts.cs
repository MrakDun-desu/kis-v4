using FluentValidation;
using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class CompositeAmounts {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("composite-amounts", ReadAll)
            .AddValidation<CompositeAmountReadAllRequest>();
    }

    public static async Task<Results<Ok<CompositeAmountReadAllResponse>, ValidationProblem>> ReadAll(
            CompositeAmountService service,
            [AsParameters] CompositeAmountReadAllRequest req,
            CancellationToken token = default
            ) {
        return TypedResults.Ok(await service.ReadAllAsync(req, token));
    }
}
