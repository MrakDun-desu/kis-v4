using KisV4.App.Auth;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class CashBoxes {
    private const string ReadRouteName = "CashBoxRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("cashboxes");
        group.MapGet(string.Empty, ReadAll)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapPost(string.Empty, Create)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapPut("{id:int}", Update)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapGet("{id:int}", Read)
            .WithName(ReadRouteName)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapDelete("{id:int}", Delete)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapPost("{id:int}/stock-taking", AddStockTaking)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
    }

    private static List<CashBoxListModel> ReadAll(
        ICashBoxService cashBoxService,
        [FromQuery] bool? deleted = false
    ) {
        return cashBoxService.ReadAll(deleted);
    }

    private static CreatedAtRoute<CashBoxDetailModel> Create(
        ICashBoxService cashBoxService,
        CashBoxCreateModel createModel
    ) {
        var createdModel = cashBoxService.Create(createModel);
        return TypedResults.CreatedAtRoute(createdModel, ReadRouteName, new { id = createdModel.Id });
    }

    private static Results<NoContent, NotFound> Update(
        ICashBoxService cashBoxService,
        CashBoxCreateModel createModel,
        int id
    ) {
        return cashBoxService.Update(id, createModel)
            .Match<Results<NoContent, NotFound>>(
                static _ => TypedResults.NoContent(),
                static _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok<CashBoxDetailModel>, NotFound, ValidationProblem> Read(
        ICashBoxService cashBoxService,
        int id,
        [FromQuery] DateTimeOffset? startDate,
        [FromQuery] DateTimeOffset? endDate
    ) {
        return cashBoxService.Read(id, startDate, endDate)
            .Match<Results<Ok<CashBoxDetailModel>, NotFound, ValidationProblem>>(
                static readModel => TypedResults.Ok(readModel),
                static _ => TypedResults.NotFound(),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static NoContent Delete(
        ICashBoxService cashBoxService,
        int id
    ) {
        cashBoxService.Delete(id);
        return TypedResults.NoContent();
    }

    private static Results<Ok, NotFound> AddStockTaking(
        ICashBoxService cashBoxService,
        int id
    ) {
        return cashBoxService.AddStockTaking(id)
            .Match<Results<Ok, NotFound>>(
                static _ => TypedResults.Ok(),
                static _ => TypedResults.NotFound()
            );
    }
}
