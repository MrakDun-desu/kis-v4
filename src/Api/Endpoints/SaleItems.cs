using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

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

    public static async Task<Results<Ok<SaleItemReadAllResponse>, ValidationProblem>> ReadAll(
        [AsParameters] SaleItemReadAllRequest req,
        IValidator<SaleItemReadAllRequest> validator,
        SaleItemService service,
        CancellationToken token = default
    ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var data = await service.ReadAllAsync(req, token);
        return TypedResults.Ok(data);
    }

    public static async Task<Results<Ok<SaleItemReadResponse>, NotFound>> Read(
        int id,
        SaleItemService service,
        CancellationToken token = default
    ) {
        return await service.ReadAsync(id, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<CreatedAtRoute<SaleItemCreateResponse>, ValidationProblem>> Create(
        SaleItemCreateRequest req,
        SaleItemService service,
        IValidator<SaleItemCreateRequest> validator,
        CancellationToken token = default
    ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var output = await service.CreateAsync(req, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<Ok<SaleItemUpdateResponse>, NotFound, ValidationProblem>> Update(
        int id,
        SaleItemUpdateRequest req,
        IValidator<SaleItemUpdateRequest> validator,
        SaleItemService service,
        CancellationToken token = default
    ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return await service.UpdateAsync(id, req, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
        int id,
        SaleItemService service,
        CancellationToken token = default
    ) {
        return await service.DeleteAsync(id, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
