using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class SaleItems {
    private const string ReadRouteName = "SaleItemsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("sale-items", ReadAll);
        routeBuilder.MapPost("sale-items", Create);
        routeBuilder.MapGet("sale-items/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPut("sale-items/{id:int}", Update);
        routeBuilder.MapDelete("sale-items/{id:int}", Delete);
    }

    public static SaleItemReadAllResponse ReadAll([AsParameters] SaleItemReadAllRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<SaleItemReadResponse>, NotFound> Read(int id) {
        throw new NotImplementedException();
    }

    public static Results<CreatedAtRoute<SaleItemCreateResponse>, ValidationProblem> Create(SaleItemCreateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Ok<SaleItemUpdateResponse>, NotFound, ValidationProblem> Update(int id, SaleItemUpdateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
