using System.Security.Claims;
using FluentValidation;
using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Containers {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("containers", ReadAll)
            .WithName("ContainersReadAll")
            .AddValidation<ContainerReadAllRequest>();
        routeBuilder.MapPost("containers", Create)
            .WithName("ContainersCreate")
            .AddValidation<ContainerCreateRequest>();
        routeBuilder.MapGet("containers/{id:int}", Read)
            .WithName("ContainersRead");
        routeBuilder.MapGet("containers/{id:int}/operator", OperatorRead)
            .WithName("ContainersOperatorRead");
        routeBuilder.MapPut("containers/{id:int}", Update)
            .WithName("ContainersUpdate")
            .AddValidation<ContainerUpdateRequest>();
    }

    public static async Task<Results<Ok<ContainerReadAllResponse>, ValidationProblem>> ReadAll(
        ContainerService service,
        [AsParameters] ContainerReadAllRequest req,
        CancellationToken token = default
    ) {
        return TypedResults.Ok(await service.ReadAllAsync(req, token));
    }

    public static async Task<Results<Ok<ContainerReadResponse>, NotFound>> Read(
        ContainerService service,
        int id,
        CancellationToken token = default
    ) {
        return await service.ReadAsync(id, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<Ok<ContainerOperatorReadResponse>, NotFound>> OperatorRead(
        int id,
        ContainerService service,
        CancellationToken token = default
    ) {
        return await service.ReadOperatorAsync(id, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<Ok<ContainerCreateResponse>, ValidationProblem>> Create(
        ContainerService service,
        ClaimsPrincipal user,
        ContainerCreateRequest req,
        CancellationToken token = default
    ) {
        return TypedResults.Ok(await service.CreateAsync(req, user.GetUserId(), token));
    }

    public static async Task<Results<Ok<ContainerUpdateResponse>, NotFound, ValidationProblem>> Update(
        ContainerService service,
        ClaimsPrincipal user,
        [AsParameters]
        ContainerUpdateRequest req,
        CancellationToken token = default
    ) {
        return await service.UpdateAsync(req, user.GetUserId(), token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }
}
