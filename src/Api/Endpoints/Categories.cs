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

    public static CategoryReadAllResponse ReadAll(CategoryService service) {
        return service.ReadAll();
    }

    public static Results<Ok<CategoryCreateResponse>, ValidationProblem> Create(
            CategoryService service,
            IValidator<CategoryCreateRequest> validator,
            CategoryCreateRequest req) {
        var validationResult = validator.Validate(req);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return TypedResults.Ok(service.Create(req));
    }

    public static Results<NoContent, NotFound, ValidationProblem> Update(
            CategoryService service,
            IValidator<CategoryUpdateRequest> validator,
            int id,
            CategoryUpdateRequest req) {
        var validationResult = validator.Validate(req);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return service.Update(id, req) ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    public static Results<NoContent, NotFound> Delete(
            CategoryService service,
            int id) {
        return service.Delete(id) ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
