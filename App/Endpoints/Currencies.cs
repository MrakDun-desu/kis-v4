using KisV4.BL.Common;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Currencies {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("currencies");
        group.MapPost(string.Empty, Create);
        group.MapGet(string.Empty, ReadAll);
        group.MapPut("{id:int}", Update);
    }

    private static int Create(
        ICurrencyService currencyService,
        CurrencyCreateModel createModel) {
        var createdId = currencyService.Create(createModel);
        return createdId;
    }

    private static List<CurrencyReadAllModel> ReadAll(ICurrencyService currencyService) {
        return currencyService.ReadAll();
    }

    private static Results<Ok, NotFound> Update(
        ICurrencyService currencyService,
        int id,
        CurrencyUpdateModel updateModel) {
        return currencyService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}
