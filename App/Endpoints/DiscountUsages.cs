using KisV4.BL.Common;
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
        var discountUsage = discountUsageService.Read(id);

        return discountUsage is null ? TypedResults.NotFound() : TypedResults.Ok(discountUsage);
    }
}