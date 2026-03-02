using System.Security.Claims;
using FluentValidation;
using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class ContainerChanges {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("container-changes", ReadAll)
            .AddValidation<ContainerChangeReadAllRequest>();
        routeBuilder.MapPost("container-changes", Create)
            .AddValidation<ContainerChangeCreateRequest>();
    }

    public static async Task<Results<Ok<ContainerChangeReadAllResponse>, ValidationProblem>> ReadAll(
            ContainerChangeService service,
            [AsParameters] ContainerChangeReadAllRequest req,
            CancellationToken token = default
            ) {
        return TypedResults.Ok(await service.ReadAll(req, token));
    }

    public static async Task<Results<Ok<ContainerChangeCreateResponse>, ValidationProblem>> Create(
            ContainerChangeService service,
            ClaimsPrincipal user,
            ContainerChangeCreateRequest req,
            CancellationToken token = default
            ) {
        return TypedResults.Ok(await service.Create(req, user.GetUserId(), token));
    }
}
