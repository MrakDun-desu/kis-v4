using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class CashBoxes
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("cashboxes");
        group.MapGet(string.Empty, ReadAll);
        group.MapPost(string.Empty, Create);
        group.MapPut(string.Empty, Update);
        group.MapGet("{id:int}", Read);
        group.MapDelete("{id:int}", Delete);
        group.MapPost("{id:int}/stock-taking", AddStockTaking);
    }

    private static List<CashBoxReadAllModel> ReadAll(
        ICashBoxService cashBoxService,
        [FromQuery] bool? deleted = false
    )
    {
        return cashBoxService.ReadAll(deleted);
    }

    private static Created<CashBoxReadModel> Create(
        ICashBoxService cashBoxService,
        CashBoxCreateModel createModel,
        HttpRequest request)
    {
        var createdModel = cashBoxService.Create(createModel);
        return TypedResults.Created(
            request.Host + request.Path + "/" + createdModel.Id, createdModel);
    }

    private static Results<NoContent, NotFound> Update(
        ICashBoxService cashBoxService,
        CashBoxUpdateModel updateModel)
    {
        return cashBoxService.Update(updateModel) ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static Results<Ok<CashBoxReadModel>, NotFound> Read(
        ICashBoxService cashBoxService,
        int id,
        [FromQuery] DateTimeOffset? startDate = null,
        [FromQuery] DateTimeOffset? endDate = null)
    {
        var cashBoxDetail = cashBoxService.Read(id, startDate, endDate);
        if (cashBoxDetail is null) return TypedResults.NotFound();

        return TypedResults.Ok(cashBoxDetail);
    }

    private static Results<NoContent, NotFound> Delete(
        ICashBoxService cashBoxService,
        int id)
    {
        return cashBoxService.Delete(id) ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> AddStockTaking(
        ICashBoxService cashBoxService,
        int id)
    {
        return cashBoxService.AddStockTaking(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}