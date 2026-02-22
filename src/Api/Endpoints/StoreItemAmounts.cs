using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class StoreItemAmounts {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("store-item-amounts", ReadAll);
    }

    public static async Task<Results<Ok<StoreItemAmountReadAllResponse>, ValidationProblem>> ReadAll(
            [AsParameters] StoreItemAmountReadAllRequest req,
            StoreItemAmountService service,
            IValidator<StoreItemAmountReadAllRequest> validator,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.ReadAllAsync(req, token));
    }
}
