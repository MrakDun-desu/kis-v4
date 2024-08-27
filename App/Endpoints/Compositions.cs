using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Compositions
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("compositions");
        group.MapPost(string.Empty, Create);
    }

    private static Results<Ok, ValidationProblem> Create(
        ICompositionService compositionService,
        CompositionCreateModel createModel)
    {
        return compositionService.Create(createModel)
            .Match<Results<Ok, ValidationProblem>>(
                _ => TypedResults.Ok(),
                errors => TypedResults.ValidationProblem(errors)
            );
    }
}