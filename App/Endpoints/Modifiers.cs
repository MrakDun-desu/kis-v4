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

    private static Results<Ok<ModifierDetailModel>, ValidationProblem> Create(
        IModifierService modifierService,
        ModifierCreateModel createModel)
    {
        return modifierService.Create(createModel)
            .Match<Results<Ok<ModifierDetailModel>, ValidationProblem>>(
                output => TypedResults.Ok(output),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<ModifierDetailModel>, NotFound> Read(
        IModifierService modifierService, int id)
    {
        return modifierService.Read(id)
            .Match<Results<Ok<ModifierDetailModel>, NotFound>>(
                output => TypedResults.Ok(output),
                _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok<ModifierDetailModel>, NotFound, ValidationProblem> Update(
        IModifierService modifierService,
        int id,
        ModifierCreateModel updateModel)
    {
        return modifierService.Update(id, updateModel)
            .Match<Results<Ok<ModifierDetailModel>, NotFound, ValidationProblem>>(
                output => TypedResults.Ok(output),
                _ => TypedResults.NotFound(),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<ModifierDetailModel>, NotFound> Delete(
        IModifierService modifierService,
        int id)
    {
        return modifierService.Delete(id)
            .Match<Results<Ok<ModifierDetailModel>, NotFound>>(
                output => TypedResults.Ok(output),
                _ => TypedResults.NotFound()
            );
    }
}