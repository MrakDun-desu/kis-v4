using KisV4.App.Auth;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Currencies {
    private const string ReadAllRouteName = "CurrencyReadAll";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("currencies");
        group.MapGet(string.Empty, ReadAll)
            .WithName(ReadAllRouteName)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapPost(string.Empty, Create)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapPut("{id:int}", Update)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
    }

    private static CreatedAtRoute<CurrencyListModel> Create(
        ICurrencyService currencyService,
        CurrencyCreateModel createModel
    ) {
        var createdModel = currencyService.Create(createModel);
        return TypedResults.CreatedAtRoute(createdModel, ReadAllRouteName);
    }

    private static IEnumerable<CurrencyListModel> ReadAll(ICurrencyService currencyService) {
        return currencyService.ReadAll();
    }

    private static Results<Ok<CurrencyListModel>, NotFound> Update(
        ICurrencyService currencyService,
        CurrencyCreateModel updateModel,
        int id
    ) {
        return currencyService.Update(id, updateModel)
            .Match<Results<Ok<CurrencyListModel>, NotFound>>(
                static model => TypedResults.Ok(model),
                static _ => TypedResults.NotFound()
            );
    }
}
