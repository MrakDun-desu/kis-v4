using KisV4.BL.Common;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Pipes {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("pipes");
        group.MapPost(string.Empty, Create);
        group.MapGet(string.Empty, ReadAll);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static int Create(
        IPipeService cashBoxService,
        PipeCreateModel createModel) {
        var createdId = cashBoxService.Create(createModel);
        return createdId;
    }

    private static List<PipeReadAllModel> ReadAll(IPipeService cashBoxService) {
        return cashBoxService.ReadAll();
    }

    private static Results<Ok, NotFound> Update(
        IPipeService cashBoxService,
        int id,
        PipeUpdateModel updateModel) {
        return cashBoxService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> Delete(
        IPipeService cashBoxService,
        int id) {
        return cashBoxService.Delete(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}
