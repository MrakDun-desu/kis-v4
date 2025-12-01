using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class CompositeAmounts {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("composite-amounts", ReadAll);
    }

    public static Results<Ok<CompositeAmountReadAllResponse>, ValidationProblem> ReadAll(
            CompositeAmountService service,
            IValidator<CompositeAmountReadAllRequest> validator,
            [AsParameters] CompositeAmountReadAllRequest req
            ) {
        var validationResult = validator.Validate(req);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(service.ReadAll(req));
    }
}
