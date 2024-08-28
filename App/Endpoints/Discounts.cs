using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Discounts
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("discounts");
        group.MapGet(string.Empty, ReadAll);
        group.MapGet("{id:int}", Read);
    }

    private static List<DiscountReadAllModel> ReadAll(IDiscountService discountService)
    {
        return discountService.ReadAll();
    }

    private static Results<Ok<DiscountReadModel>, NotFound> Read(
        IDiscountService discountService,
        int id)
    {
        return discountService.Read(id)
            .Match<Results<Ok<DiscountReadModel>, NotFound>>(
                result => TypedResults.Ok(result),
                _ => TypedResults.NotFound()
            );
    }
}