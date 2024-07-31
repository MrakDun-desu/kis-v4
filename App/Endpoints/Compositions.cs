using KisV4.BL.Common.Services;
using KisV4.Common.Models;

namespace KisV4.App.Endpoints;

public static class Compositions {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("compositions");
        group.MapPost(string.Empty, Create);
    }

    private static void Create(
        ICompositionService compositionService,
        CompositionCreateModel createModel) {
        compositionService.Create(createModel);
    }
}
