using System.Security.Claims;
using FluentValidation;
using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Costs {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapPost("costs", Create)
            .AddValidation<CostCreateRequest>();
    }

    public static async Task<Results<Ok<CostCreateResponse>, ValidationProblem>> Create(
            CostCreateRequest req,
            CostService service,
            ClaimsPrincipal claims,
            CancellationToken token = default
            ) {
        return TypedResults.Ok(await service.Create(req, claims.GetUserId(), token));
    }
}
