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

    public static Results<Ok<ContainerReadAllResponse>, ValidationProblem> ReadAll(
            ContainerService service,
            IValidator<ContainerReadAllRequest> validator,
            [AsParameters] ContainerReadAllRequest req
            ) {
        var validationResult = validator.Validate(req);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(service.ReadAll(req));
    }

    public static Results<Ok<ContainerReadResponse>, NotFound> Read(
            ContainerService service,
            int id
            ) {
        return service.Read(id) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static Results<Ok<ContainerCreateResponse>, ValidationProblem> Create(
            ContainerService service,
            IValidator<ContainerCreateRequest> validator,
            ClaimsPrincipal user,
            ContainerCreateRequest req) {
        var validationResult = validator.Validate(req);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(service.Create(req, user.GetUserId()));
    }

    public static Results<Ok<ContainerUpdateResponse>, NotFound, ValidationProblem> Update(
            ContainerService service,
            IValidator<ContainerUpdateRequest> validator,
            ClaimsPrincipal user,
            int id,
            ContainerUpdateRequest req) {

        var validationResult = validator.Validate(req);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return service.Update(id, req, user.GetUserId()) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }
}
