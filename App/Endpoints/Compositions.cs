using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Compositions {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("compositions");
        group.MapPut(string.Empty, CreateOrUpdate);
    }

    private static Results<Ok<CompositionListModel>, NoContent, ValidationProblem> CreateOrUpdate(
        ICompositionService compositionService,
        CompositionCreateModel createModel) {
        return compositionService.CreateOrUpdate(createModel)
            .Match<Results<Ok<CompositionListModel>, NoContent, ValidationProblem>>(
                _ => TypedResults.NoContent(),
                model => TypedResults.Ok(model),
                errors => TypedResults.ValidationProblem(errors)
            );
    }
}
