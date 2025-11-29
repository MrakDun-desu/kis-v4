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

    public static Results<Ok<CompositionReadAllResponse>, ValidationProblem> ReadAll(
            CompositionService service,
            IValidator<CompositionReadAllRequest> validator,
            [AsParameters] CompositionReadAllRequest req
            ) {
        var validationResult = validator.Validate(req);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(service.ReadAll(req));
    }

    public static Results<ValidationProblem, NoContent> Put(
            CompositionService service,
            IValidator<CompositionPutRequest> validator,
            CompositionPutRequest req
            ) {
        var validationResult = validator.Validate(req);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        service.Put(req);

        return TypedResults.NoContent();
    }

}
