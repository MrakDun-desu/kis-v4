using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class StoreItems
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("store-items");
        group.MapPost(string.Empty, Create);
        group.MapGet(string.Empty, ReadAll);
        group.MapGet("{id:int}", Read);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static int Create(
        IStoreItemService storeItemService,
        StoreItemCreateModel createModel)
    {
        var createdId = storeItemService.Create(createModel);
        return createdId;
    }

    private static List<StoreItemListModel> ReadAll(IStoreItemService storeItemService)
    {
        return storeItemService.ReadAll();
    }

    private static Results<Ok<StoreItemDetailModel>, NotFound> Read(IStoreItemService storeItemService, int id)
    {
        var storeItemModel = storeItemService.Read(id);

        return storeItemModel is null ? TypedResults.NotFound() : TypedResults.Ok(storeItemModel);
    }

    private static Results<Ok, NotFound> Update(
        IStoreItemService storeItemService,
        int id,
        StoreItemCreateModel updateModel)
    {
        return storeItemService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> Delete(
        IStoreItemService storeItemService,
        int id)
    {
        return storeItemService.Delete(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}