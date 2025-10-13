using KisV4.App.Auth;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.App.Endpoints;

public static class CurrencyChanges {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("currency-changes");
        group.MapGet(string.Empty, Create);
    }

    private static Results<Ok<Page<CurrencyChangeListModel>>, ValidationProblem> Create(
        ICurrencyChangeService currencyChangeService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] int? accountId,
        [FromQuery] bool? cancelled,
        [FromQuery] DateTimeOffset? startDate,
        [FromQuery] DateTimeOffset? endDate
    ) {
        return currencyChangeService.ReadAll(page, pageSize, accountId, cancelled, startDate, endDate)
            .Match<Results<Ok<Page<CurrencyChangeListModel>>, ValidationProblem>>(
                static output => TypedResults.Ok(output),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }
}
