using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class StoreItems {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("store-items");
        group.MapGet(string.Empty, ReadAll);
        group.MapPost(string.Empty, Create);
        group.MapGet("{id:int}", Read);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static Results<Ok<Page<StoreItemListModel>>, ValidationProblem> ReadAll(
        IStoreItemService storeItemService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] bool? deleted,
        [FromQuery] int? categoryId,
        [FromQuery] int? storeId
    ) {
        return storeItemService.ReadAll(page, pageSize, deleted, categoryId, storeId)
            .Match<Results<Ok<Page<StoreItemListModel>>, ValidationProblem>>(
                output => TypedResults.Ok(output),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<StoreItemDetailModel>, ValidationProblem> Create(
        IStoreItemService storeItemService,
        StoreItemCreateModel createModel) {
        return storeItemService.Create(createModel)
            .Match<Results<Ok<StoreItemDetailModel>, ValidationProblem>>(
                output => TypedResults.Ok(output),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<StoreItemDetailModel>, NotFound> Read(
        IStoreItemService storeItemService,
        int id) {
        return storeItemService.Read(id)
            .Match<Results<Ok<StoreItemDetailModel>, NotFound>>(
                output => TypedResults.Ok(output),
                _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok<StoreItemDetailModel>, NotFound, ValidationProblem> Update(
        IStoreItemService storeItemService,
        StoreItemCreateModel updateModel,
        int id) {
        return storeItemService.Update(id, updateModel)
            .Match<Results<Ok<StoreItemDetailModel>, NotFound, ValidationProblem>>(
                output => TypedResults.Ok(output),
                _ => TypedResults.NotFound(),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<StoreItemDetailModel>, NotFound> Delete(
        IStoreItemService storeItemService,
        int id) {
        return storeItemService.Delete(id)
            .Match<Results<Ok<StoreItemDetailModel>, NotFound>>(
                output => TypedResults.Ok(output),
                _ => TypedResults.NotFound()
            );
    }
}
