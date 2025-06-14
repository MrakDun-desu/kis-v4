using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Currencies {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("currencies");
        group.MapGet(string.Empty, ReadAll);
        group.MapPost(string.Empty, Create);
        group.MapPut("{id:int}", Update);
    }

    private static CurrencyListModel Create(
        ICurrencyService currencyService,
        CurrencyCreateModel createModel) {
        return currencyService.Create(createModel);
    }

    private static IEnumerable<CurrencyListModel> ReadAll(ICurrencyService currencyService) {
        return currencyService.ReadAll();
    }

    private static Results<Ok<CurrencyListModel>, NotFound> Update(
        ICurrencyService currencyService,
        CurrencyCreateModel updateModel,
        int id) {
        return currencyService.Update(id, updateModel)
            .Match<Results<Ok<CurrencyListModel>, NotFound>>(
                model => TypedResults.Ok(model),
                _ => TypedResults.NotFound()
            );
    }
}
