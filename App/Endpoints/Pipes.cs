using KisV4.BL.Common.Services;
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

    private static PipeListModel Create(
        IPipeService cashBoxService,
        PipeCreateModel createModel) {
        return cashBoxService.Create(createModel);
    }

    private static List<PipeListModel> ReadAll(IPipeService cashBoxService) {
        return cashBoxService.ReadAll();
    }

    private static Results<Ok<PipeListModel>, NotFound> Update(
        IPipeService cashBoxService,
        int id,
        PipeCreateModel updateModel) {
        return cashBoxService.Update(id, updateModel)
            .Match<Results<Ok<PipeListModel>, NotFound>>(
                output => TypedResults.Ok(output),
                _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok, NotFound, BadRequest<string>> Delete(
        IPipeService cashBoxService,
        int id) {
        return cashBoxService.Delete(id)
            .Match<Results<Ok, NotFound, BadRequest<string>>>(
                _ => TypedResults.Ok(),
                _ => TypedResults.NotFound(),
                error => TypedResults.BadRequest(error)
            );
    }
}
