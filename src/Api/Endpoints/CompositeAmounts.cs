using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class CompositeAmounts {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("composite-amounts", ReadAll);
    }

    public static CompositeAmountReadAllResponse ReadAll(
            [AsParameters] CompositeAmountReadAllRequest req
            ) {
        throw new NotImplementedException();
    }
}
