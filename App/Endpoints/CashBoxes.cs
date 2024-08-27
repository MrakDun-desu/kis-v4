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
        return cashBoxService.Update(updateModel)
            .Match<Results<NoContent, NotFound>>(
                _ => TypedResults.NoContent(),
                _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok<CashBoxReadModel>, NotFound> Read(
        ICashBoxService cashBoxService,
        int id,
        [FromQuery] DateTimeOffset? startDate,
        [FromQuery] DateTimeOffset? endDate)
    {
        return cashBoxService.Read(id, startDate, endDate)
            .Match<Results<Ok<CashBoxReadModel>, NotFound>>(
                readModel => TypedResults.Ok(readModel),
                _ => TypedResults.NotFound()
            );
    }

    private static Results<NoContent, NotFound> Delete(
        ICashBoxService cashBoxService,
        int id)
    {
        return cashBoxService.Delete(id)
            .Match<Results<NoContent, NotFound>>(
                _ => TypedResults.NoContent(),
                _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok, NotFound> AddStockTaking(
        ICashBoxService cashBoxService,
        int id)
    {
        return cashBoxService.AddStockTaking(id)
            .Match<Results<Ok, NotFound>>(
                _ => TypedResults.Ok(),
                _ => TypedResults.NotFound()
            );
    }
}