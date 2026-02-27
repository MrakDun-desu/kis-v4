using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Modifiers {
    private const string ReadRouteName = "ModifiersRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("modifiers", ReadAll);
        routeBuilder.MapGet("modifers/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("modifiers", Create);
        routeBuilder.MapPut("modifiers/{id:int}", Update);
        routeBuilder.MapDelete("modifiers/{id:int}", Delete);
    }

    public static async Task<Results<Ok<ModifierReadAllResponse>, ValidationProblem>> ReadAll(
        [AsParameters] ModifierReadAllRequest req,
        ModifierService service,
        IValidator<ModifierReadAllRequest> validator,
        CancellationToken token = default
    ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.ReadAllAsync(req, token));
    }

    public static async Task<Results<Ok<ModifierReadResponse>, NotFound>> Read(
        int id,
        ModifierService service,
        CancellationToken token = default
    ) {
        return await service.ReadAsync(id, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<CreatedAtRoute<ModifierCreateResponse>, ValidationProblem>> Create(
        ModifierCreateRequest req,
        ModifierService service,
        IValidator<ModifierCreateRequest> validator,
        CancellationToken token = default
    ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var output = await service.CreateAsync(req, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<Ok<ModifierUpdateResponse>, NotFound, ValidationProblem>> Update(
        int id,
        ModifierUpdateRequest req,
        IValidator<ModifierUpdateRequest> validator,
        ModifierService service,
        CancellationToken token = default
    ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var output = await service.UpdateAsync(id, req, token);
        return TypedResults.Ok(output);
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
        int id,
        ModifierService service,
        CancellationToken token = default
    ) {
        return await service.DeleteAsync(id, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
