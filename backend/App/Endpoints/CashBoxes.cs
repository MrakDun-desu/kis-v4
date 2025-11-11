using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class CashBoxes {
    private const string ReadRouteName = "CashBoxesRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("cashboxes", ReadAll);
        routeBuilder.MapPost("cashboxes", Create);
        routeBuilder.MapPost("cashboxes/{id:int}", StockTaking);
        routeBuilder.MapGet("cashboxes/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPut("cashboxes/{id:int}", Update);
        routeBuilder.MapDelete("cashboxes/{id:int}", Delete);
    }

    public static CashBoxReadAllResponse ReadAll() {
        throw new NotImplementedException();
    }

    public static CreatedAtRoute<CashBoxCreateResponse> Create(CashBoxCreateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<Created<StockTakingCreateResponse>, NotFound> StockTaking(int id) {
        throw new NotImplementedException();
    }

    public static Results<Ok<CashBoxReadResponse>, NotFound> Read(int id) {
        throw new NotImplementedException();
    }

    public static Results<Ok<CashBoxUpdateResponse>, NotFound> Update(int id, CashBoxUpdateRequest req) {
        throw new NotImplementedException();
    }

    public static Results<NoContent, NotFound> Delete(int id) {
        throw new NotImplementedException();
    }
}
