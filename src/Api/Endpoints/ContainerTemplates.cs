using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class ContainerTemplates {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("container-templates", ReadAll)
            .AddValidation<ContainerTemplateReadAllRequest>();
        routeBuilder.MapPost("container-templates", Create)
            .AddValidation<ContainerTemplateCreateRequest>();
        routeBuilder.MapPut("container-templates/{id:int}", Update)
            .AddValidation<ContainerTemplateUpdateRequest>();
        routeBuilder.MapDelete("container-templates/{id:int}", Delete);
    }

    public static async Task<Results<Ok<ContainerTemplateReadAllResponse>, ValidationProblem>> ReadAll(
            [AsParameters] ContainerTemplateReadAllRequest req,
            ContainerTemplateService service,
            CancellationToken token = default
            ) {
        return TypedResults.Ok(await service.ReadAllAsync(req, token));
    }

    public static async Task<Results<Ok<ContainerTemplateCreateResponse>, ValidationProblem>> Create(
            ContainerTemplateCreateRequest req,
            ContainerTemplateService service,
            CancellationToken token = default
            ) {
        return TypedResults.Ok(await service.CreateAsync(req, token));
    }

    public static async Task<Results<Ok<ContainerTemplateUpdateResponse>, NotFound, ValidationProblem>> Update(
        [AsParameters]
        ContainerTemplateUpdateRequest req,
            ContainerTemplateService service,
            CancellationToken token = default
            ) {
        return await service.UpdateAsync(req, token) switch {
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
