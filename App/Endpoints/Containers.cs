using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Containers {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("containers");
        group.MapPost(string.Empty, Create);
        group.MapGet(string.Empty, ReadAll);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static int Create(
        IContainerService containerService,
        ContainerCreateModel createModel) {
        return containerService.Create(createModel);
    }

    private static List<ContainerReadAllModel> ReadAll(IContainerService containerService) {
        return containerService.ReadAll();
    }

    private static Results<Ok, NotFound> Update(
        IContainerService containerService,
        int id,
        ContainerUpdateModel updateModel) {
        return containerService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> Delete(IContainerService containerService, int id) {
        return containerService.Delete(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}
