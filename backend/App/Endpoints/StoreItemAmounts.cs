using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class StoreItemAmounts {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("store-item-amounts", ReadAll);
    }

    public static StoreItemAmountReadAllResponse ReadAll([AsParameters] StoreItemAmountReadAllRequest req) {
        throw new NotImplementedException();
    }
}
