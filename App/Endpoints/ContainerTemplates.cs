using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class ContainerTemplates
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("container-templates");
        group.MapGet(string.Empty, ReadAll);
        group.MapPost(string.Empty, Create);
        group.MapPatch("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static Results<Ok<ICollection<ContainerTemplateListModel>>, ValidationProblem> ReadAll(
        IContainerTemplateService containerTemplateService,
        [FromQuery] bool? deleted,
        [FromQuery] int? containedItemId)
    {
        return containerTemplateService
            .ReadAll(deleted, containedItemId)
            .Match<Results<Ok<ICollection<ContainerTemplateListModel>>, ValidationProblem>>(
                output => TypedResults.Ok(output),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Created<ContainerTemplateListModel>, ValidationProblem> Create(
        IContainerTemplateService containerTemplateService,
        ContainerTemplateCreateModel createModel,
        HttpRequest request)
    {
        var creationResult = containerTemplateService.Create(createModel);
        return creationResult.Match<Results<Created<ContainerTemplateListModel>, ValidationProblem>>(
            createdModel => TypedResults.Created(
                request.Host + request.Path + "/" + createdModel.Id,
                createdModel),
            errors => TypedResults.ValidationProblem(errors)
        );
    }

    private static Results<NoContent, NotFound, ValidationProblem> Update(
        IContainerTemplateService containerTemplateService,
        ContainerTemplateCreateModel updateModel,
        int id)
    {
        return containerTemplateService.Update(id, updateModel)
            .Match<Results<NoContent, NotFound, ValidationProblem>>(
                _ => TypedResults.NoContent(),
                _ => TypedResults.NotFound(),
                errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<NoContent, NotFound> Delete(
        IContainerTemplateService containerTemplateService,
        int id
    )
    {
        return containerTemplateService.Delete(id)
            .Match<Results<NoContent, NotFound>>(
                _ => TypedResults.NoContent(),
                _ => TypedResults.NotFound()
            );
    }
}