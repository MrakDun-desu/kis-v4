using System.Security.Claims;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class Containers
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("containers");
        group.MapGet(string.Empty, ReadAll);
        group.MapPost(string.Empty, Create);
        group.MapPatch("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static Results<Ok<Page<ContainerListModel>>, ValidationProblem> ReadAll(
        IContainerService containerService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] bool? deleted,
        [FromQuery] int? pipeId)
    {
        return containerService
            .ReadAll(page, pageSize, deleted, pipeId)
            .Match<Results<Ok<Page<ContainerListModel>>, ValidationProblem>>(
                output => TypedResults.Ok(output),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Created<ContainerListModel>, ValidationProblem> Create(
        IContainerService containerService,
        ContainerCreateModel createModel,
        HttpRequest request,
        ClaimsPrincipal claims)
    {
        var creationResult = containerService.Create(createModel, claims.Identity!.Name!);
        return creationResult.Match<Results<Created<ContainerListModel>, ValidationProblem>>(
            createdModel => TypedResults.Created(
                request.Host + request.Path + "/" + createdModel.Id,
                createdModel),
            errors => TypedResults.ValidationProblem(errors)
        );
    }

    private static Results<Ok<ContainerListModel>, NotFound, ValidationProblem> Update(
        IContainerService containerService,
        ContainerPatchModel updateModel,
        int id)
    {
        return containerService.Patch(id, updateModel)
            .Match<Results<Ok<ContainerListModel>, NotFound, ValidationProblem>>(
                model => TypedResults.Ok(model),
                _ => TypedResults.NotFound(),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<ContainerListModel>, NotFound> Delete(
        IContainerService containerService,
        int id,
        ClaimsPrincipal claims
    )
    {
        return containerService.Delete(id, claims.Identity!.Name!)
            .Match<Results<Ok<ContainerListModel>, NotFound>>(
                model => TypedResults.Ok(model),
                _ => TypedResults.NotFound()
            );
    }
}