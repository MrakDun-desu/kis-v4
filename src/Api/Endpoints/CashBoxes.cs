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

    public static async Task<CashBoxReadAllResponse> ReadAll(
            CashBoxService service,
            CancellationToken token = default
            ) {
        return await service.ReadAllAsync(token);
    }

    public static async Task<Results<CreatedAtRoute<CashBoxCreateResponse>, ValidationProblem>> Create(
            CashBoxService service,
            CashBoxCreateRequest req,
            IValidator<CashBoxCreateRequest> validator,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var output = await service.CreateAsync(req, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<Ok<StockTakingCreateResponse>, NotFound>> StockTaking(
            CashBoxService service,
            ClaimsPrincipal claims,
            int id,
            CancellationToken token = default
            ) {
        return await service.StockTakingAsync(id, claims.GetUserId(), token) switch {
            null => TypedResults.NotFound(),
            var response => TypedResults.Ok(response)
        };
    }

    public static async Task<Results<Ok<CashBoxReadResponse>, NotFound>> Read(
            CashBoxService service,
            int id,
            CancellationToken token = default
            ) {
        return await service.ReadAsync(id, token) switch {
            null => TypedResults.NotFound(),
            var response => TypedResults.Ok(response)
        };
    }

    public static async Task<Results<Ok<CashBoxUpdateResponse>, NotFound, ValidationProblem>> Update(
            CashBoxService service,
            int id,
            CashBoxUpdateRequest req,
            IValidator<CashBoxUpdateRequest> validator,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);

        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return await service.UpdateAsync(id, req, token) switch {
            null => TypedResults.NotFound(),
            var response => TypedResults.Ok(response)
        };
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
            CashBoxService service,
            int id,
            CancellationToken token = default
            ) {
        return await service.DeleteAsync(id, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
