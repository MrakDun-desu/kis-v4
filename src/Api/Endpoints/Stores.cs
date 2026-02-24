using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

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

    public static async Task<StoreReadAllResponse> ReadAll(
            StoreService service,
            CancellationToken token = default
            ) {
        return await service.ReadAllAsync(token);
    }

    public static async Task<Results<Ok<StoreReadResponse>, NotFound>> Read(
            int id,
            StoreService service,
            CancellationToken token = default
            ) {
        return await service.ReadAsync(id, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<CreatedAtRoute<StoreCreateResponse>, ValidationProblem>> Create(
            StoreCreateRequest req,
            IValidator<StoreCreateRequest> validator,
            StoreService service,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var output = await service.CreateAsync(req, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<Ok<StoreUpdateResponse>, NotFound, ValidationProblem>> Update(
            int id,
            StoreUpdateRequest req,
            IValidator<StoreUpdateRequest> validator,
            StoreService service,
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
            StoreService service,
            CancellationToken token = default
            ) {
        return await service.DeleteAsync(id, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
