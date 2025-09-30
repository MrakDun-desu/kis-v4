using KisV4.App.Auth;
using KisV4.App.Configuration;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KisV4.App.Endpoints;

public static class DiscountUsages {
    private const string ReadAllRouteName = "DiscountUsagesReadAll";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("discount-usages");
        group.MapGet(string.Empty, ReadAll)
            .WithName(ReadAllRouteName)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapPost(string.Empty, Create)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
        group.MapGet("{id:int}", Read)
            .RequireAuthorization(p => p.RequireRole(RoleNames.Admin));
    }

    private static Results<Ok<Page<DiscountUsageListModel>>, ValidationProblem> ReadAll(
        IDiscountUsageService discountUsageService,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] int? discountId,
        [FromQuery] int? userId
    ) {
        return discountUsageService.ReadAll(page, pageSize, discountId, userId)
            .Match<Results<Ok<Page<DiscountUsageListModel>>, ValidationProblem>>(
                static output => TypedResults.Ok(output),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<CreatedAtRoute<DiscountUsageDetailModel>, ValidationProblem> Create(
        IDiscountUsageService discountUsageService,
        DiscountUsageCreateModel createModel,
        IOptions<ScriptStorageSettings> conf
        ) {
        return discountUsageService.Create(createModel, conf.Value.Path)
            .Match<Results<CreatedAtRoute<DiscountUsageDetailModel>, ValidationProblem>>(
                static createdModel => TypedResults.CreatedAtRoute(
                    createdModel,
                    ReadAllRouteName
                ),
                static errors => TypedResults.ValidationProblem(errors)
            );
    }

    private static Results<Ok<DiscountUsageDetailModel>, NotFound> Read
        (IDiscountUsageService discountUsageService, int id) {
        return discountUsageService.Read(id)
            .Match<Results<Ok<DiscountUsageDetailModel>, NotFound>>(
                static result => TypedResults.Ok(result),
                static _ => TypedResults.NotFound()
            );
    }
}
