using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class CompositeAmounts {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("composite-amounts", ReadAll);
    }

    public static async Task<Results<Ok<CompositeAmountReadAllResponse>, ValidationProblem>> ReadAll(
            CompositeAmountService service,
            IValidator<CompositeAmountReadAllRequest> validator,
            [AsParameters] CompositeAmountReadAllRequest req,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.ReadAllAsync(req, token));
    }
}
