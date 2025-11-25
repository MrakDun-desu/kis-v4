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

    public static ContainerTemplateReadAllResponse ReadAll([AsParameters] ContainerTemplateReadAllRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Created<ContainerTemplateCreateResponse>, ValidationProblem> Create(ContainerTemplateCreateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<ContainerTemplateUpdateResponse>, NotFound, ValidationProblem> Update(int id, ContainerTemplateUpdateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
