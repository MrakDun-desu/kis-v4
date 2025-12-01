using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Compositions {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("compositions", ReadAll);
        routeBuilder.MapPut("compositions", Put);
    }

    public static async Task<Results<Ok<CompositionReadAllResponse>, ValidationProblem>> ReadAll(
            CompositionService service,
            IValidator<CompositionReadAllRequest> validator,
            [AsParameters] CompositionReadAllRequest req,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.ReadAllAsync(req, token));
    }

    public static async Task<Results<ValidationProblem, NoContent>> Put(
            CompositionService service,
            IValidator<CompositionPutRequest> validator,
            CompositionPutRequest req,
            CancellationToken token
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        await service.Put(req, token);

        return TypedResults.NoContent();
    }

}
