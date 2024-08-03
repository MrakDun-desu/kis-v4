using KisV4.BL.Common;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Modifiers {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("modifiers");
        group.MapPost(string.Empty, Create);
        group.MapGet("{id:int}", Read);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static int Create(
        IModifierService ModifierService,
        ModifierCreateModel createModel) {
        var createdId = ModifierService.Create(createModel);
        return createdId;
    }

    private static Results<Ok<ModifierReadModel>, NotFound> Read(IModifierService ModifierService, int id) {
        var ModifierModel = ModifierService.Read(id);

        return ModifierModel is null ? TypedResults.NotFound() : TypedResults.Ok(ModifierModel);
    }

    private static Results<Ok, NotFound> Update(
        IModifierService ModifierService,
        int id,
        ModifierUpdateModel updateModel) {
        return ModifierService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> Delete(
        IModifierService ModifierService,
        int id) {
        return ModifierService.Delete(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}
