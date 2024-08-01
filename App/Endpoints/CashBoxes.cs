using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class CashBoxes {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("cashboxes");
        group.MapPost(string.Empty, Create);
        group.MapPost("{id:int}/stock-taking", AddStockTaking);
        group.MapGet(string.Empty, ReadAll);
        group.MapGet("{id:int}", Read);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static int Create(
        ICashBoxService cashBoxService,
        CashBoxCreateModel createModel) {
        var createdId = cashBoxService.Create(createModel);
        return createdId;
    }

    private static Results<Ok, NotFound>AddStockTaking(
        ICashBoxService cashBoxService,
        int id) {
        return cashBoxService.AddStockTaking(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static List<CashBoxReadAllModel> ReadAll(ICashBoxService cashBoxService) {
        return cashBoxService.ReadAll();
    }

    private static Results<Ok<CashBoxReadModel>, NotFound> Read(
        ICashBoxService cashBoxService,
        int id) {
        var cashBoxDetail = cashBoxService.Read(id);
        if (cashBoxDetail is null) {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(cashBoxDetail);
    }

    private static Results<Ok, NotFound> Update(
        ICashBoxService cashBoxService,
        int id,
        CashBoxUpdateModel updateModel) {
        return cashBoxService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> Delete(
        ICashBoxService cashBoxService,
        int id) {
        return cashBoxService.Delete(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}
