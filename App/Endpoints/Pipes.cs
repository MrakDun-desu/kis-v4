using KisV4.App.Auth;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Pipes {
    private const string ReadAllRouteName = "PipesReadAll";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("pipes");
        group.MapPost(string.Empty, Create)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapGet(string.Empty, ReadAll)
            .WithName(ReadAllRouteName)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapPut("{id:int}", Update)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapDelete("{id:int}", Delete)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
    }

    private static CreatedAtRoute<PipeListModel> Create(
        IPipeService cashBoxService,
        PipeCreateModel createModel) {
        var createdModel = cashBoxService.Create(createModel);
        return TypedResults.CreatedAtRoute(
            createdModel,
            ReadAllRouteName
        );
    }

    private static List<PipeListModel> ReadAll(IPipeService cashBoxService) {
        return cashBoxService.ReadAll();
    }

    private static Results<Ok<PipeListModel>, NotFound> Update(
        IPipeService cashBoxService,
        int id,
        PipeCreateModel updateModel
    ) {
        return cashBoxService.Update(id, updateModel)
            .Match<Results<Ok<PipeListModel>, NotFound>>(
                static output => TypedResults.Ok(output),
                static _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok, NotFound, BadRequest<string>> Delete(
        IPipeService cashBoxService,
        int id
    ) {
        return cashBoxService.Delete(id)
            .Match<Results<Ok, NotFound, BadRequest<string>>>(
                static _ => TypedResults.Ok(),
                static _ => TypedResults.NotFound(),
                static error => TypedResults.BadRequest(error)
            );
    }
}
