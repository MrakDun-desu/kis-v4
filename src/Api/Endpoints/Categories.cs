using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Categories {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("categories", ReadAll);
        routeBuilder.MapPost("categories", Create);
        routeBuilder.MapPut("categories/{id:int}", Update);
        routeBuilder.MapDelete("categories/{id:int}", Delete);
    }

    public static CategoryReadAllResponse ReadAll() {
        throw new NotImplementedException();
    }

    public static Created<CategoryCreateResponse> Create(CategoryCreateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Update(int id, CategoryUpdateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
