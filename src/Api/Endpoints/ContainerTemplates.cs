using KisV4.Api.RouteFilters;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class ContainerTemplates {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("container-templates", ReadAll)
            .WithName("ContainerTemplatesReadAll")
            .AddValidation<ContainerTemplateReadAllRequest>();
        routeBuilder.MapPost("container-templates", Create)
            .WithName("ContainerTemplatesCreate")
            .AddValidation<ContainerTemplateCreateRequest>();
        routeBuilder.MapPut("container-templates/{id:int}", Update)
            .WithName("ContainerTemplatesUpdate")
            .AddValidation<ContainerTemplateUpdateRequest>();
        routeBuilder.MapDelete("container-templates/{id:int}", Delete)
            .AddValidation<ContainerTemplateDeleteRequest>()
            .WithName("ContainerTemplatesDelete");
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
            [AsParameters]
            ContainerTemplateDeleteRequest req,
            ContainerTemplateService service,
            CancellationToken token = default) {
        return await service.DeleteAsync(req, token) ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
