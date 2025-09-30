using KisV4.App.Auth;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Costs {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("costs");
        group.MapPost(string.Empty, Create)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
    }

    private static Results<Ok<CostListModel>, ValidationProblem> Create(
        ICostService costService,
        CostCreateModel createModel) {
        return costService.Create(createModel)
            .Match<Results<Ok<CostListModel>, ValidationProblem>>(
                static model => TypedResults.Ok(model),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }
}
