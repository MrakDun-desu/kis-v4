using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class SaleItems
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("sale-items");
        group.MapGet(string.Empty, ReadAll);
        group.MapPost(string.Empty, Create);
        group.MapGet("{id:int}", Read);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static Results<Ok<Page<SaleItemListModel>>, ValidationProblem> ReadAll(
        ISaleItemService saleItemService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] bool? deleted,
        [FromQuery] int? categoryId,
        [FromQuery] bool? showOnWeb)
    {
        return saleItemService.ReadAll(page, pageSize, deleted, categoryId, showOnWeb)
            .Match<Results<Ok<Page<SaleItemListModel>>, ValidationProblem>>(
                output => TypedResults.Ok(output),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<SaleItemDetailModel>, ValidationProblem> Create(
        ISaleItemService saleItemService,
        SaleItemCreateModel createModel)
    {
        return saleItemService.Create(createModel)
            .Match<Results<Ok<SaleItemDetailModel>, ValidationProblem>>(
                output => TypedResults.Ok(output),
                errors => TypedResults.ValidationProblem(errors)
            );
    }


    private static Results<Ok<SaleItemDetailModel>, NotFound> Read(ISaleItemService saleItemService, int id)
    {
        return saleItemService.Read(id)
            .Match<Results<Ok<SaleItemDetailModel>, NotFound>>(
                output => TypedResults.Ok(output),
                _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok<SaleItemDetailModel>, NotFound, ValidationProblem> Update(
        ISaleItemService saleItemService,
        int id,
        SaleItemCreateModel updateModel)
    {
        return saleItemService.Update(id, updateModel)
            .Match<Results<Ok<SaleItemDetailModel>, NotFound, ValidationProblem>>(
                output => TypedResults.Ok(output),
                _ => TypedResults.NotFound(),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<SaleItemDetailModel>, NotFound> Delete(
        ISaleItemService saleItemService,
        int id)
    {
        return saleItemService.Delete(id)
            .Match<Results<Ok<SaleItemDetailModel>, NotFound>>(
                output => TypedResults.Ok(output),
                _ => TypedResults.NotFound()
            );
    }
}