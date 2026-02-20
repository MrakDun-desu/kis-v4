using System.Security.Claims;
using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Costs {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("costs", ReadAll);
        routeBuilder.MapPost("costs", Create);
    }

    public static async Task<Results<Ok<CostReadAllResponse>, ValidationProblem>> ReadAll(
            [AsParameters] CostReadAllRequest req,
            CostService service,
            IValidator<CostReadAllRequest> validator,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req);

        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.ReadAll(req, token));
    }

    public static async Task<Results<Ok<CostCreateResponse>, ValidationProblem>> Create(
            CostCreateRequest req,
            CostService service,
            IValidator<CostCreateRequest> validator,
            ClaimsPrincipal claims,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req);

        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.Create(req, claims.GetUserId(), token));
    }
}
