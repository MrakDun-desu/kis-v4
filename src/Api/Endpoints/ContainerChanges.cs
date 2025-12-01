using System.Security.Claims;
using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class ContainerChanges {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("container-changes", ReadAll);
        routeBuilder.MapPost("container-changes", Create);
    }

    public static async Task<Results<Ok<ContainerChangeReadAllResponse>, ValidationProblem>> ReadAll(
            ContainerChangeService service,
            IValidator<ContainerChangeReadAllRequest> validator,
            [AsParameters] ContainerChangeReadAllRequest req,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.ReadAll(req, token));
    }

    public static async Task<Results<Ok<ContainerChangeCreateResponse>, ValidationProblem>> Create(
            ContainerChangeService service,
            IValidator<ContainerChangeCreateRequest> validator,
            ClaimsPrincipal user,
            ContainerChangeCreateRequest req,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.Create(req, user.GetUserId(), token));
    }
}
