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

    private static Results<ValidationProblem, Ok> Create(
        ICompositionService compositionService,
        CompositionCreateModel createModel)
    {
        var result = compositionService.Create(createModel);
        if (result is not null)
        {
            return TypedResults.ValidationProblem(result);
        }

        return TypedResults.Ok();
    }
}