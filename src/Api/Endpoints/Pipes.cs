using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Pipes {
    private const string ReadRouteName = "PipesRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("pipes", ReadAll);
        routeBuilder.MapGet("pipes/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapPost("pipes", Create);
        routeBuilder.MapPut("pipes/{id:int}", Update);
        routeBuilder.MapDelete("pipes/{id:int}", Delete);
    }

    public static async Task<PipeReadAllResponse> ReadAll(
        PipeService service,
        CancellationToken token = default
    ) {
        return await service.ReadAllAsync(token);
    }

    public static async Task<Results<Ok<PipeReadResponse>, NotFound>> Read(
        int id,
        PipeService service,
        CancellationToken token = default
    ) {
        return await service.ReadAsync(id, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    public static async Task<Results<CreatedAtRoute<PipeCreateResponse>, ValidationProblem>> Create(
        PipeCreateRequest req,
        PipeService service,
        IValidator<PipeCreateRequest> validator,
        CancellationToken token = default
    ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var output = await service.CreateAsync(req, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    public static async Task<Results<Ok<PipeUpdateResponse>, NotFound, ValidationProblem>> Update(
        int id,
        PipeUpdateRequest req,
        PipeService service,
        IValidator<PipeUpdateRequest> validator,
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
        PipeService service,
        CancellationToken token = default
    ) {
        return await service.DeleteAsync(id, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
