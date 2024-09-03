using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Modifiers
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("modifiers");
        group.MapPost(string.Empty, Create);
        group.MapGet("{id:int}", Read);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static int Create(
        IModifierService modifierService,
        ModifierCreateModel createModel)
    {
        var createdId = modifierService.Create(createModel);
        return createdId;
    }

    private static Results<Ok<ModifierDetailModel>, NotFound> Read(IModifierService modifierService, int id)
    {
        var modifierModel = modifierService.Read(id);

        return modifierModel is null ? TypedResults.NotFound() : TypedResults.Ok(modifierModel);
    }

    private static Results<Ok, NotFound> Update(
        IModifierService modifierService,
        int id,
        ModifierCreateModel updateModel)
    {
        return modifierService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> Delete(
        IModifierService modifierService,
        int id)
    {
        return modifierService.Delete(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}