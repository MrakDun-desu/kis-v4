using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class StoreItems {
    private const string ReadRouteName = "StoreItemsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("store-items", ReadAll);
        routeBuilder.MapPost("store-items", Create);
        routeBuilder.MapGet("store-items/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPut("store-items/{id:int}", Update);
        routeBuilder.MapDelete("store-items/{id:int}", Delete);
    }

    public static StoreItemReadAllResponse ReadAll([AsParameters] StoreItemReadAllRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<StoreItemReadResponse>, NotFound> Read(int id) {
        throw new NotImplementedException();
    }

    public static Results<CreatedAtRoute<StoreItemCreateResponse>, ValidationProblem> Create(StoreItemCreateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<StoreItemUpdateResponse>, NotFound, ValidationProblem> Update(int id, StoreItemUpdateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
