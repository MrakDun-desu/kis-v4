using FluentValidation;
using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Compositions {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("compositions", ReadAll)
            .WithName("CompositionsReadAll")
            .AddValidation<CompositionReadAllRequest>();
        routeBuilder.MapPut("compositions", Put)
            .WithName("CompositionsPut")
            .AddValidation<CompositionPutRequest>();
    }

    public static async Task<Results<Ok<CompositionReadAllResponse>, ValidationProblem>> ReadAll(
            CompositionService service,
            [AsParameters] CompositionReadAllRequest req,
            CancellationToken token = default
            ) {
        return TypedResults.Ok(await service.ReadAllAsync(req, token));
    }

    public static async Task<Results<ValidationProblem, NoContent>> Put(
            CompositionService service,
            IValidator<CompositionPutRequest> validator,
            CompositionPutRequest req,
            CancellationToken token
            ) {
        await service.Put(req, token);

        return TypedResults.NoContent();
    }

}
