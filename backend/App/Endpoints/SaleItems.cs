using KisV4.App.Auth;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class SaleItems {
    private const string ReadRouteName = "SaleItemsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("sale-items");
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

    private static Results<Ok<Page<SaleItemListModel>>, ValidationProblem> ReadAll(
        ISaleItemService saleItemService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] bool? deleted,
        [FromQuery] int? categoryId,
        [FromQuery] bool? showOnWeb
    ) {
        return saleItemService.ReadAll(page, pageSize, deleted, categoryId, showOnWeb)
            .Match<Results<Ok<Page<SaleItemListModel>>, ValidationProblem>>(
                static output => TypedResults.Ok(output),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<CreatedAtRoute<SaleItemDetailModel>, ValidationProblem> Create(
        ISaleItemService saleItemService,
        SaleItemCreateModel createModel
    ) {
        return saleItemService.Create(createModel)
            .Match<Results<CreatedAtRoute<SaleItemDetailModel>, ValidationProblem>>(
                static createdModel => TypedResults.CreatedAtRoute(
                    createdModel,
                    ReadRouteName,
                    new { id = createdModel.Id }
                ),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }


    private static Results<Ok<SaleItemDetailModel>, NotFound> Read(
        ISaleItemService saleItemService,
        int id
    ) {
        return saleItemService.Read(id)
            .Match<Results<Ok<SaleItemDetailModel>, NotFound>>(
                static output => TypedResults.Ok(output),
                static _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok<SaleItemDetailModel>, NotFound, ValidationProblem> Update(
        ISaleItemService saleItemService,
        int id,
        SaleItemCreateModel updateModel
    ) {
        return saleItemService.Update(id, updateModel)
            .Match<Results<Ok<SaleItemDetailModel>, NotFound, ValidationProblem>>(
                static output => TypedResults.Ok(output),
                static _ => TypedResults.NotFound(),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<SaleItemDetailModel>, NotFound> Delete(
        ISaleItemService saleItemService,
        int id
    ) {
        return saleItemService.Delete(id)
            .Match<Results<Ok<SaleItemDetailModel>, NotFound>>(
                static output => TypedResults.Ok(output),
                static _ => TypedResults.NotFound()
            );
    }
}
