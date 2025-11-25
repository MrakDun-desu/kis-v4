using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Compositions {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("compositions", ReadAll);
        routeBuilder.MapPut("compositions", Put);
    }

    public static Results<Ok<CompositionReadAllResponse>, NotFound> ReadAll([AsParameters] CompositionReadAllRequest req) {
        throw new NotImplementedException();
    }

    public static Results<ValidationProblem, NoContent> Put(CompositionPutRequest req) {
        throw new NotImplementedException();
    }

}
