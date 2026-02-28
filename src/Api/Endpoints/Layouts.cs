using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KisV4.Api.Endpoints;

public static class Layouts {
    private const string ReadRouteName = "LayoutsRead";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("layouts", ReadAll);
        routeBuilder.MapGet("layouts/{id:int}", Read)
            .WithName(ReadRouteName);
        routeBuilder.MapGet("layouts/top-level", ReadTopLevel);
        routeBuilder.MapPost("layouts", Create);
        routeBuilder.MapPut("layouts/{id:int}", Update);
        routeBuilder.MapDelete("layouts/{id:int}", Delete);
    }

    private static async Task<LayoutReadAllResponse> ReadAll(
        [AsParameters] LayoutReadAllRequest req,
        LayoutService service,
        CancellationToken token = default
    ) {
        return await service.ReadAllAsync(req, token);
    }

    private static async Task<Results<Ok<LayoutReadResponse>, ValidationProblem>> Read(
        int id,
        [FromQuery] int? storeId,
        LayoutService service,
        IValidator<LayoutReadCommand> validator,
        CancellationToken token = default
    ) {
        var command = new LayoutReadCommand(
            Id: id,
            StoreId: storeId
        );
        var validationResult = await validator.ValidateAsync(command, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.ReadAsync(command, token));
    }

    private static async Task<Results<Ok<LayoutReadResponse>, NotFound, ValidationProblem>> ReadTopLevel(
        [FromQuery] int? storeId,
        IValidator<LayoutReadTopLevelCommand> validator,
        LayoutService service,
        CancellationToken token = default
    ) {
        var command = new LayoutReadTopLevelCommand(
            StoreId: storeId
        );
        var validationResult = await validator.ValidateAsync(command, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return await service.ReadTopLevelAsync(command, token) switch {
            null => TypedResults.NotFound(),
            var val => TypedResults.Ok(val)
        };
    }

    private static async Task<Results<CreatedAtRoute<LayoutCreateResponse>, ValidationProblem>> Create(
        [FromQuery] int? storeId,
        LayoutCreateRequest req,
        IValidator<LayoutCreateCommand> validator,
        LayoutService service,
        CancellationToken token = default
    ) {
        var command = new LayoutCreateCommand(
            Request: req,
            StoreId: storeId
        );
        var validationResult = await validator.ValidateAsync(command, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var output = await service.CreateAsync(command, token);
        return TypedResults.CreatedAtRoute(output, ReadRouteName, new { id = output.Id });
    }

    private static async Task<Results<Ok<LayoutUpdateResponse>, NotFound, ValidationProblem>> Update(
        int id,
        [FromQuery] int? storeId,
        LayoutUpdateRequest req,
        IValidator<LayoutUpdateCommand> validator,
        LayoutService service,
        CancellationToken token = default
    ) {
        var command = new LayoutUpdateCommand(
            Id: id,
            StoreId: storeId,
            Request: req
        );
        var validationResult = await validator.ValidateAsync(command, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var output = await service.UpdateAsync(command, token);
        return TypedResults.Ok(output);
    }

    private static async Task<Results<NoContent, NotFound>> Delete(
        int id,
        LayoutService service,
        CancellationToken token = default
    ) {
        return await service.DeleteAsync(id, token)
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
