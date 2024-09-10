using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class DiscountUsages
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("discount-usages");
        group.MapGet(string.Empty, ReadAll);
        // group.MapPost(string.Empty, Create);
        group.MapGet("{id:int}", Read);
    }

    private static Results<Ok<Page<DiscountUsageListModel>>, ValidationProblem> ReadAll(
        IDiscountUsageService discountUsageService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] int? discountId,
        [FromQuery] int? userId
    )
    {
        return discountUsageService.ReadAll(page, pageSize, discountId, userId)
            .Match<Results<Ok<Page<DiscountUsageListModel>>, ValidationProblem>>(
                output => TypedResults.Ok(output),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    // private static Results<Ok<DiscountUsageDetailModel>, ValidationProblem> Create(
    //     IDiscountUsageService discountUsageService,
    //     DiscountUsageCreateModel createModel)
    // {
    //     return discountUsageService.Create(createModel)
    //         .Match<Results<Ok<DiscountUsageDetailModel>, ValidationProblem>>(
    //             output => TypedResults.Ok(output),
    //             errors => TypedResults.ValidationProblem(errors)
    //         );
    // }
    //
    private static Results<Ok<DiscountUsageDetailModel>, NotFound> Read
        (IDiscountUsageService discountUsageService, int id)
    {
        return discountUsageService.Read(id)
            .Match<Results<Ok<DiscountUsageDetailModel>, NotFound>>(
                result => TypedResults.Ok(result),
                _ => TypedResults.NotFound()
            );
    }
}