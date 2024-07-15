using KisV4.BL.Common;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class CashBoxes {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("cashboxes");
        group.MapPost(string.Empty, Create);
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

    private static List<CashBoxListModel> ReadAll(ICashBoxService cashBoxService) {
        return cashBoxService.ReadAll();
    }

    private static Results<Ok<CashBoxDetailModel>, NotFound>Read(
        ICashBoxService cashBoxService,
        int id) {
        var cashboxDetail = cashBoxService.Read(id);
        if (cashboxDetail is null) {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(cashboxDetail);
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
