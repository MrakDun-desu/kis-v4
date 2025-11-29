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

    public static Results<Ok<ContainerChangeReadAllResponse>, ValidationProblem> ReadAll(
            ContainerChangeService service,
            IValidator<ContainerChangeReadAllRequest> validator,
            [AsParameters] ContainerChangeReadAllRequest req
            ) {
        var validationResult = validator.Validate(req);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(service.ReadAll(req));
    }

    public static Results<Ok<ContainerChangeCreateResponse>, ValidationProblem> Create(
            ContainerChangeService service,
            IValidator<ContainerChangeCreateRequest> validator,
            ClaimsPrincipal user,
            ContainerChangeCreateRequest req
            ) {
        var validationResult = validator.Validate(req);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(service.Create(req, user.GetUserId()));
    }
}
