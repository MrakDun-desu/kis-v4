using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class Discounts
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("discounts");
        group.MapGet(string.Empty, ReadAll);
        group.MapGet("{id:int}", Read);
        group.MapPatch("{id:int}", Patch);
        group.MapDelete("{id:int}", Delete);
    }

    private static IEnumerable<DiscountListModel> ReadAll(
        IDiscountService discountService,
        [FromQuery] bool? deleted
    )
    {
        return discountService.ReadAll(deleted);
    }

    private static Results<Ok<DiscountDetailModel>, NotFound> Read(
        IDiscountService discountService,
        int id
    )
    {
        return discountService.Read(id).Match<Results<Ok<DiscountDetailModel>, NotFound>>(
            result => TypedResults.Ok(result),
            _ => TypedResults.NotFound()
        );
    }

    private static Results<Ok<DiscountDetailModel>, NotFound> Patch(
        IDiscountService discountService,
        int id
    )
    {
        return discountService.Patch(id).Match<Results<Ok<DiscountDetailModel>, NotFound>>(
            result => TypedResults.Ok(result),
            _ => TypedResults.NotFound()
        );
    }

    private static Results<Ok<DiscountDetailModel>, NotFound> Delete(
        IDiscountService discountService,
        int id
    )
    {
        return discountService.Delete(id).Match<Results<Ok<DiscountDetailModel>, NotFound>>(
            result => TypedResults.Ok(result),
            _ => TypedResults.NotFound()
        );
    }
}