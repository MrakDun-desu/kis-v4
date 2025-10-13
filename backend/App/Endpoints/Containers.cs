using System.Security.Claims;
using KisV4.App.Auth;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class Containers {
    private const string ReadAllRouteName = "ContainersReadAll";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("containers");
        group.MapGet(string.Empty, ReadAll)
            .WithName(ReadAllRouteName);
        group.MapPost(string.Empty, Create);
        group.MapPatch("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static Results<Ok<Page<ContainerListModel>>, ValidationProblem> ReadAll(
        IContainerService containerService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] bool? deleted,
        [FromQuery] int? pipeId
    ) {
        return containerService
            .ReadAll(page, pageSize, deleted, pipeId)
            .Match<Results<Ok<Page<ContainerListModel>>, ValidationProblem>>(
                static output => TypedResults.Ok(output),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<CreatedAtRoute<ContainerListModel>, ValidationProblem> Create(
        IContainerService containerService,
        ContainerCreateModel createModel,
        ClaimsPrincipal claims
    ) {
        var creationResult = containerService.Create(createModel, claims.Identity!.Name!);
        return creationResult.Match<Results<CreatedAtRoute<ContainerListModel>, ValidationProblem>>(
            static createdModel => TypedResults.CreatedAtRoute(
                createdModel,
                ReadAllRouteName
                ),
            static errors => TypedResults.ValidationProblem(errors)
        );
    }

    private static Results<Ok<ContainerListModel>, NotFound, ValidationProblem> Update(
        IContainerService containerService,
        ContainerPatchModel updateModel,
        int id
    ) {
        return containerService.Patch(id, updateModel)
            .Match<Results<Ok<ContainerListModel>, NotFound, ValidationProblem>>(
                static model => TypedResults.Ok(model),
                static _ => TypedResults.NotFound(),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<ContainerListModel>, NotFound> Delete(
        IContainerService containerService,
        int id,
        ClaimsPrincipal claims
    ) {
        return containerService.Delete(id, claims.Identity!.Name!)
            .Match<Results<Ok<ContainerListModel>, NotFound>>(
                static model => TypedResults.Ok(model),
                static _ => TypedResults.NotFound()
            );
    }
}
