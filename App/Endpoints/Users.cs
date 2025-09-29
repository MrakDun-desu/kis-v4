using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class Users {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("users");
        group.MapGet(string.Empty, ReadAll);
        group.MapGet("{id:int}", Read);
    }

    private static Results<Ok<Page<UserListModel>>, ValidationProblem> ReadAll(
        IUserService userService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] bool? deleted
    ) {
        return userService.ReadAll(page, pageSize, deleted).Match<Results<Ok<Page<UserListModel>>, ValidationProblem>>(
            static models => TypedResults.Ok(models),
            static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<UserDetailModel>, NotFound, ValidationProblem> Read(
        IUserService userService,
        int id
    ) {
        return userService.Read(id).Match<Results<Ok<UserDetailModel>, NotFound, ValidationProblem>>(
            static model => TypedResults.Ok(model),
            static _ => TypedResults.NotFound(),
            static errors => TypedResults.ValidationProblem(errors)
        );
    }
}
