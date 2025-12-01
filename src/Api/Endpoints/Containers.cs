using System.Security.Claims;
using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Containers {
    private const string ReadRouteName = "ContainersRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("containers", ReadAll);
        routeBuilder.MapPost("containers", Create);
        routeBuilder.MapGet("containers/{id:int}", Read);
        routeBuilder.MapPut("containers/{id:int}", Update);
    }

    public static async Task<Results<Ok<ContainerReadAllResponse>, ValidationProblem>> ReadAll(
            ContainerService service,
            IValidator<ContainerReadAllRequest> validator,
            [AsParameters] ContainerReadAllRequest req,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

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

    public static async Task<Results<Ok<ContainerCreateResponse>, ValidationProblem>> Create(
            ContainerService service,
            IValidator<ContainerCreateRequest> validator,
            ClaimsPrincipal user,
            ContainerCreateRequest req,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.CreateAsync(req, user.GetUserId(), token));
    }

    public static async Task<Results<Ok<ContainerUpdateResponse>, NotFound, ValidationProblem>> Update(
            ContainerService service,
            IValidator<ContainerUpdateRequest> validator,
            ClaimsPrincipal user,
            int id,
            ContainerUpdateRequest req,
            CancellationToken token = default
            ) {

        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return await service.UpdateAsync(id, req, user.GetUserId(), token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }
}
