using KisV4.App.Auth;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Modifiers {
    private const string ReadRouteName = "ModifiersRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("modifiers");
        group.MapPost(string.Empty, Create)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapGet("{id:int}", Read)
            .WithName(ReadRouteName)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapPut("{id:int}", Update)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapDelete("{id:int}", Delete)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
    }

    private static Results<CreatedAtRoute<ModifierDetailModel>, ValidationProblem> Create(
        IModifierService modifierService,
        ModifierCreateModel createModel
    ) {
        return modifierService.Create(createModel)
            .Match<Results<CreatedAtRoute<ModifierDetailModel>, ValidationProblem>>(
                static createdModel => TypedResults.CreatedAtRoute(
                    createdModel,
                    ReadRouteName,
                    new { id = createdModel.Id }
                ),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<ModifierDetailModel>, NotFound> Read(
        IModifierService modifierService, int id
    ) {
        return modifierService.Read(id)
            .Match<Results<Ok<ModifierDetailModel>, NotFound>>(
                static output => TypedResults.Ok(output),
                static _ => TypedResults.NotFound()
            );
    }

    private static Results<Ok<ModifierDetailModel>, NotFound, ValidationProblem> Update(
        IModifierService modifierService,
        int id,
        ModifierCreateModel updateModel
    ) {
        return modifierService.Update(id, updateModel)
            .Match<Results<Ok<ModifierDetailModel>, NotFound, ValidationProblem>>(
                static output => TypedResults.Ok(output),
                static _ => TypedResults.NotFound(),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<ModifierDetailModel>, NotFound> Delete(
        IModifierService modifierService,
        int id
    ) {
        return modifierService.Delete(id)
            .Match<Results<Ok<ModifierDetailModel>, NotFound>>(
                static output => TypedResults.Ok(output),
                static _ => TypedResults.NotFound()
            );
    }
}
