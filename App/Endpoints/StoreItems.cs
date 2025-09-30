using KisV4.App.Auth;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class StoreItems {
    private const string ReadRouteName = "StoreItemsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("store-items");
        group.MapGet(string.Empty, ReadAll)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapPost(string.Empty, Create)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapGet("{id:int}", Read)
            .WithName(ReadRouteName)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapPut("{id:int}", Update)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapDelete("{id:int}", Delete)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
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
                static output => TypedResults.Ok(output),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<CreatedAtRoute<StoreItemDetailModel>, ValidationProblem> Create(
        IStoreItemService storeItemService,
        StoreItemCreateModel createModel
    ) {
        return storeItemService.Create(createModel)
            .Match<Results<CreatedAtRoute<StoreItemDetailModel>, ValidationProblem>>(
                static createdModel => TypedResults.CreatedAtRoute(
                    createdModel,
                    ReadRouteName,
                    new { id = createdModel.Id }
                ),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<StoreItemDetailModel>, NotFound> Read(
        IStoreItemService storeItemService,
        int id
    ) {
        return storeItemService.Read(id)
            .Match<Results<Ok<StoreItemDetailModel>, NotFound>>(
                static output => TypedResults.Ok(output),
                static _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok<StoreItemDetailModel>, NotFound, ValidationProblem> Update(
        IStoreItemService storeItemService,
        StoreItemCreateModel updateModel,
        int id
    ) {
        return storeItemService.Update(id, updateModel)
            .Match<Results<Ok<StoreItemDetailModel>, NotFound, ValidationProblem>>(
                static output => TypedResults.Ok(output),
                static _ => TypedResults.NotFound(),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<StoreItemDetailModel>, NotFound> Delete(
        IStoreItemService storeItemService,
        int id
    ) {
        return storeItemService.Delete(id)
            .Match<Results<Ok<StoreItemDetailModel>, NotFound>>(
                static output => TypedResults.Ok(output),
                static _ => TypedResults.NotFound()
            );
    }
}
