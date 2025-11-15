using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Stores {
    private const string ReadRouteName = "StoresRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("stores", ReadAll);
        routeBuilder.MapGet("stores/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("stores", Create);
        routeBuilder.MapPut("stores/{id:int}", Update);
        routeBuilder.MapDelete("stores/{id:int}", Delete);
    }

    public static StoreReadAllResponse ReadAll() {
        throw new NotImplementedException();
    }

    public static StoreReadResponse Read(int id) {
        throw new NotImplementedException();
    }

    public static CreatedAtRoute<StoreCreateResponse> Create(StoreCreateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<StoreUpdateResponse>, NotFound> Update(int id, StoreUpdateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
