using Api.BL.Common;
using KisV4.Api.Common.Models.Cashbox;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Cashboxes {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("cashboxes");
        group.MapPost(string.Empty, Create);
        group.MapGet(string.Empty, ReadAll);
        group.MapGet("{id:int}", Read);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static int Create(
        ICashboxService cashboxService,
        CashboxCreateModel createModel) {
        var createdId = cashboxService.Create(createModel);
        return createdId;
    }

    private static List<CashboxListModel> ReadAll(ICashboxService cashboxService) {
        return cashboxService.ReadAll();
    }

    private static Results<Ok<CashboxDetailModel>, NotFound>Read(
        ICashboxService cashboxService,
        int id) {
        var cashboxDetail = cashboxService.Read(id);
        if (cashboxDetail is null) {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(cashboxDetail);
    }

    private static Results<Ok, NotFound> Update(
        ICashboxService cashboxService,
        int id,
        CashboxUpdateModel updateModel) {
        return cashboxService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> Delete(
        ICashboxService cashboxService,
        int id) {
        return cashboxService.Delete(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}
