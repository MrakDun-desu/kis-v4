using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Stores {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("stores");
        group.MapPost(string.Empty, Create);
        group.MapGet(string.Empty, ReadAll);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static int Create(
        IStoreService cashBoxService,
        StoreCreateModel createModel) {
        var createdId = cashBoxService.Create(createModel);
        return createdId;
    }

    private static List<StoreReadAllModel> ReadAll(IStoreService cashBoxService) {
        return cashBoxService.ReadAll();
    }

    private static Results<Ok, NotFound> Update(
        IStoreService cashBoxService,
        int id,
        StoreUpdateModel updateModel) {
        return cashBoxService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> Delete(
        IStoreService cashBoxService,
        int id) {
        return cashBoxService.Delete(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}
