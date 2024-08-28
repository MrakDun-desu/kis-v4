using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Currencies
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("currencies");
        group.MapGet(string.Empty, ReadAll);
        group.MapPost(string.Empty, Create);
        group.MapPut(string.Empty, Update);
    }

    private static CurrencyReadAllModel Create(
        ICurrencyService currencyService,
        CurrencyCreateModel createModel)
    {
        return currencyService.Create(createModel);
    }

    private static List<CurrencyReadAllModel> ReadAll(ICurrencyService currencyService)
    {
        return currencyService.ReadAll();
    }

    private static Results<NoContent, NotFound> Update(
        ICurrencyService currencyService,
        CurrencyUpdateModel updateModel)
    {
        return currencyService.Update(updateModel)
            .Match<Results<NoContent, NotFound>>(
                _ => TypedResults.NoContent(),
                _ => TypedResults.NotFound()
            );
    }
}