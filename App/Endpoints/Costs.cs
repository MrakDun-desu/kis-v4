using KisV4.BL.Common.Services;
using KisV4.Common.Models;

namespace KisV4.App.Endpoints;

public static class Costs
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("costs");
        group.MapPost(string.Empty, Create);
    }
    
    public static int Create(ICostService costService, CostCreateModel createModel) {
        return costService.Create(createModel);
    }
}
