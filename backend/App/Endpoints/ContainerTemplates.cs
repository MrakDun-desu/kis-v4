using KisV4.App.Auth;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class ContainerTemplates {
    private const string ReadAllRouteName = "ContainerTemplateReadAll";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("container-templates");
        group.MapGet(string.Empty, ReadAll)
            .WithName(ReadAllRouteName);
        group.MapPost(string.Empty, Create);
        group.MapPatch("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static Results<Ok<ICollection<ContainerTemplateListModel>>, ValidationProblem> ReadAll(
        IContainerTemplateService containerTemplateService,
        [FromQuery] bool? deleted,
        [FromQuery] int? containedItemId
    ) {
        return containerTemplateService
            .ReadAll(deleted, containedItemId)
            .Match<Results<Ok<ICollection<ContainerTemplateListModel>>, ValidationProblem>>(
                static output => TypedResults.Ok(output),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<CreatedAtRoute<ContainerTemplateListModel>, ValidationProblem> Create(
        IContainerTemplateService containerTemplateService,
        ContainerTemplateCreateModel createModel,
        HttpRequest request
    ) {
        var creationResult = containerTemplateService.Create(createModel);
        return creationResult.Match<Results<CreatedAtRoute<ContainerTemplateListModel>, ValidationProblem>>(
            static createdModel => TypedResults.CreatedAtRoute(
                createdModel,
                ReadAllRouteName
                ),
            static errors => TypedResults.ValidationProblem(errors)
        );
    }

    private static Results<Ok<ContainerTemplateListModel>, NotFound, ValidationProblem> Update(
        IContainerTemplateService containerTemplateService,
        ContainerTemplateCreateModel updateModel,
        int id
    ) {
        return containerTemplateService.Update(id, updateModel)
            .Match<Results<Ok<ContainerTemplateListModel>, NotFound, ValidationProblem>>(
                static model => TypedResults.Ok(model),
                static _ => TypedResults.NotFound(),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<ContainerTemplateListModel>, NotFound> Delete(
        IContainerTemplateService containerTemplateService,
        int id
    ) {
        return containerTemplateService.Delete(id)
            .Match<Results<Ok<ContainerTemplateListModel>, NotFound>>(
                static model => TypedResults.Ok(model),
                static _ => TypedResults.NotFound()
            );
    }
}
