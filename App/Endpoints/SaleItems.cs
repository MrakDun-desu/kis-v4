using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class SaleItems
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("sale-items");
        group.MapPost(string.Empty, Create);
        group.MapGet(string.Empty, ReadAll);
        group.MapGet("{id:int}", Read);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static int Create(
        ISaleItemService saleItemService,
        SaleItemCreateModel createModel)
    {
        var createdId = saleItemService.Create(createModel);
        return createdId;
    }

    private static List<SaleItemListModel> ReadAll(ISaleItemService saleItemService)
    {
        return saleItemService.ReadAll();
    }

    private static Results<Ok<SaleItemDetailModel>, NotFound> Read(ISaleItemService saleItemService, int id)
    {
        var saleItemModel = saleItemService.Read(id);

        return saleItemModel is null ? TypedResults.NotFound() : TypedResults.Ok(saleItemModel);
    }

    private static Results<Ok, NotFound> Update(
        ISaleItemService saleItemService,
        int id,
        SaleItemCreateModel updateModel)
    {
        return saleItemService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> Delete(
        ISaleItemService saleItemService,
        int id)
    {
        return saleItemService.Delete(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}