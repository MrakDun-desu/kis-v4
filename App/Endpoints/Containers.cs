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
        group.MapPatch(string.Empty, Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static Results<Ok<Page<ContainerReadAllModel>>, ValidationProblem> ReadAll(
        IContainerService containerService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] bool? deleted,
        [FromQuery] int? pipeId)
    {
        return containerService
            .ReadAll(page, pageSize, deleted, pipeId)
            .Match<Results<Ok<Page<ContainerReadAllModel>>, ValidationProblem>>(
                output => TypedResults.Ok(output),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Created<ContainerReadAllModel>, ValidationProblem> Create(
        IContainerService containerService,
        ContainerCreateModel createModel,
        HttpRequest request,
        ClaimsPrincipal claims)
    {
        var creationResult = containerService.Create(createModel, claims.Identity!.Name!);
        return creationResult.Match<Results<Created<ContainerReadAllModel>, ValidationProblem>>(
            createdModel => TypedResults.Created(
                request.Host + request.Path + "/" + createdModel.Id,
                createdModel),
            errors => TypedResults.ValidationProblem(errors)
        );
    }

    private static Results<NoContent, NotFound, ValidationProblem> Update(
        IContainerService containerService,
        ContainerUpdateModel updateModel)
    {
        return containerService.Update(updateModel)
            .Match<Results<NoContent, NotFound, ValidationProblem>>(
                _ => TypedResults.NoContent(),
                _ => TypedResults.NotFound(),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<NoContent, NotFound> Delete(
        IContainerService containerService,
        int id,
        ClaimsPrincipal claims
    )
    {
        return containerService.Delete(id, claims.Identity!.Name!)
            .Match<Results<NoContent, NotFound>>(
                _ => TypedResults.NoContent(),
                _ => TypedResults.NotFound()
            );
    }
}