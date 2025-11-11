using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Costs {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("costs", ReadAll);
        routeBuilder.MapPost("costs", Create);
        routeBuilder.MapDelete("costs/{id:int}", Delete);
    }

    public static CostReadAllResponse ReadAll([AsParameters] CostReadAllRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Created<CostCreateResponse>, ValidationProblem> Create(CostCreateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
