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
    }

    private static List<DiscountReadAllModel> ReadAll(IDiscountService discountService)
    {
        return discountService.ReadAll();
    }
}