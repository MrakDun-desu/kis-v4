using KisV4.App.Auth;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Stores {
    private const string ReadRouteName = "StoresRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("stores");
        group.MapPost(string.Empty, Create);
        group.MapGet(string.Empty, ReadAll);
        group.MapGet("{id:int}", Read)
            .WithName(ReadRouteName);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static CreatedAtRoute<StoreDetailModel> Create(
        IStoreService storeService,
        StoreCreateModel createModel
    ) {
        var createdModel = storeService.Create(createModel);
        return TypedResults.CreatedAtRoute(
            createdModel,
            ReadRouteName,
            new { id = createdModel.Id }
        );
    }

    private static List<StoreListModel> ReadAll(IStoreService storeService) {
        return storeService.ReadAll();
    }

    private static Results<Ok<StoreDetailModel>, NotFound> Read(
        IStoreService storeService,
        int id
    ) {
        return storeService.Read(id).Match<Results<Ok<StoreDetailModel>, NotFound>>(
            static output => TypedResults.Ok(output),
            static _ => TypedResults.NotFound()
        );
    }

    private static Results<Ok, NotFound> Update(
        IStoreService storeService,
        int id,
        StoreCreateModel updateModel
    ) {
        return storeService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> Delete(
        IStoreService storeService,
        int id
    ) {
        return storeService.Delete(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}
