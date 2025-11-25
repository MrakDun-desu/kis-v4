using System.Security.Claims;
using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class CashBoxes {
    private const string ReadRouteName = "CashBoxesRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("cashboxes", ReadAll);
        routeBuilder.MapPost("cashboxes", Create);
        routeBuilder.MapPost("cashboxes/{id:int}/stock-taking", StockTaking);
        routeBuilder.MapGet("cashboxes/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPut("cashboxes/{id:int}", Update);
        routeBuilder.MapDelete("cashboxes/{id:int}", Delete);
    }

    public static CashBoxReadAllResponse ReadAll(CashBoxService service) {
        return service.ReadAll();
    }

    public static Results<CreatedAtRoute<CashBoxCreateResponse>, ValidationProblem> Create(
            CashBoxService service,
            CashBoxCreateRequest req,
            IValidator<CashBoxCreateRequest> validator
            ) {
        var validationResult = validator.Validate(req);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var output = service.Create(req);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static Results<Ok<StockTakingCreateResponse>, NotFound> StockTaking(
            CashBoxService service,
            ClaimsPrincipal claims,
            int id
            ) {
        return service.StockTaking(id, claims.GetUserId())
            .Match<Results<Ok<StockTakingCreateResponse>, NotFound>>(
                    response => TypedResults.Ok(response),
                    _ => TypedResults.NotFound()
                );

    }

    public static Results<Ok<CashBoxReadResponse>, NotFound> Read(
            CashBoxService service,
            int id
            ) {
        return service.Read(id)
            .Match<Results<Ok<CashBoxReadResponse>, NotFound>>(
                response => TypedResults.Ok(response),
                _ => TypedResults.NotFound()
            );
    }

    public static Results<Ok<CashBoxUpdateResponse>, NotFound, ValidationProblem> Update(
            CashBoxService service,
            int id,
            CashBoxUpdateRequest req,
            IValidator<CashBoxUpdateRequest> validator
            ) {
        var validationResult = validator.Validate(req);

        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return service.Update(id, req)
            .Match<Results<Ok<CashBoxUpdateResponse>, NotFound, ValidationProblem>>(
                response => TypedResults.Ok(response),
                _ => TypedResults.NotFound()
            );
    }

    public static Results<NoContent, NotFound> Delete(
            CashBoxService service,
            int id
            ) {
        return service.Delete(id) switch {
            true => TypedResults.NoContent(),
            false => TypedResults.NotFound()
        };
    }
}
