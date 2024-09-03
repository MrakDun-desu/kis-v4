using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class DiscountUsages
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("discount-usages");
        group.MapGet("{id:int}", Read);
    }

    private static Results<Ok<DiscountUsageReadModel>, NotFound> Read
        (IDiscountUsageService discountUsageService, int id)
    {
        return discountUsageService.Read(id)
            .Match<Results<Ok<DiscountUsageReadModel>, NotFound>>(
                result => TypedResults.Ok(result),
                _ => TypedResults.NotFound()
            );
    }
}