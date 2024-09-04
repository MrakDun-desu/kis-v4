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
        group.MapPut("{id:int}", Update);
        group.MapGet("{id:int}", Read);
        group.MapDelete("{id:int}", Delete);
        group.MapPost("{id:int}/stock-taking", AddStockTaking);
    }

    private static List<CashBoxListModel> ReadAll(
        ICashBoxService cashBoxService,
        [FromQuery] bool? deleted = false
    )
    {
        return cashBoxService.ReadAll(deleted);
    }

    private static Created<CashBoxDetailModel> Create(
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
        CashBoxCreateModel createModel,
        int id)
    {
        return cashBoxService.Update(id, createModel)
            .Match<Results<NoContent, NotFound>>(
                _ => TypedResults.NoContent(),
                _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok<CashBoxDetailModel>, NotFound, ValidationProblem> Read(
        ICashBoxService cashBoxService,
        int id,
        [FromQuery] DateTimeOffset? startDate,
        [FromQuery] DateTimeOffset? endDate)
    {
        return cashBoxService.Read(id, startDate, endDate)
            .Match<Results<Ok<CashBoxDetailModel>, NotFound, ValidationProblem>>(
                readModel => TypedResults.Ok(readModel),
                _ => TypedResults.NotFound(),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static NoContent Delete(
        ICashBoxService cashBoxService,
        int id)
    {
        cashBoxService.Delete(id);
        return TypedResults.NoContent();
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