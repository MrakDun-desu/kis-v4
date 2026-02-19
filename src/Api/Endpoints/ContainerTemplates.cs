using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class ContainerTemplates {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("container-templates", ReadAll);
        routeBuilder.MapPost("container-templates", Create);
        routeBuilder.MapPut("container-templates/{id:int}", Update);
        routeBuilder.MapDelete("container-templates/{id:int}", Delete);
    }

    public static async Task<ContainerTemplateReadAllResponse> ReadAll(
            [AsParameters] ContainerTemplateReadAllRequest req,
            ContainerTemplateService service,
            CancellationToken token = default
            ) {
        return await service.ReadAllAsync(req, token);
    }

    public static async Task<Results<Ok<ContainerTemplateCreateResponse>, ValidationProblem>> Create(
            ContainerTemplateCreateRequest req,
            ContainerTemplateService service,
            IValidator<ContainerTemplateCreateRequest> validator,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.CreateAsync(req, token));
    }

    public static async Task<Results<Ok<ContainerTemplateUpdateResponse>, NotFound, ValidationProblem>> Update(
            int id,
            ContainerTemplateUpdateRequest req,
            IValidator<ContainerTemplateUpdateRequest> validator,
            ContainerTemplateService service,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return await service.UpdateAsync(id, req, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
            int id,
            ContainerTemplateService service,
            CancellationToken token = default) {
        return await service.DeleteAsync(id) ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
