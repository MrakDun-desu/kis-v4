using FluentValidation;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Categories {

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapGet("categories", ReadAll);
        routeBuilder.MapPost("categories", Create);
        routeBuilder.MapPut("categories/{id:int}", Update);
        routeBuilder.MapDelete("categories/{id:int}", Delete);
    }

    public static async Task<CategoryReadAllResponse> ReadAll(CategoryService service, CancellationToken token = default) {
        return await service.ReadAllAsync(token);
    }

    public static async Task<Results<Ok<CategoryCreateResponse>, ValidationProblem>> Create(
            CategoryService service,
            IValidator<CategoryCreateRequest> validator,
            CategoryCreateRequest req,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(await service.CreateAsync(req, token));
    }

    public static async Task<Results<NoContent, NotFound, ValidationProblem>> Update(
            CategoryService service,
            IValidator<CategoryUpdateRequest> validator,
            int id,
            CategoryUpdateRequest req,
            CancellationToken token = default
            ) {
        var validationResult = await validator.ValidateAsync(req, token);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return await service.UpdateAsync(id, req, token) ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    public static async Task<Results<NoContent, NotFound>> Delete(
            CategoryService service,
            int id,
            CancellationToken token = default
            ) {
        return await service.DeleteAsync(id, token) ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
