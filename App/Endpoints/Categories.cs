using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Categories
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("categories");
        group.MapGet(string.Empty, ReadAll);
        group.MapPost(string.Empty, Create);
        group.MapPut(string.Empty, Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static List<CategoryReadAllModel> ReadAll(ICategoryService categoryService)
    {
        return categoryService.ReadAll();
    }

    private static Created<CategoryReadAllModel> Create(
        ICategoryService categoryService,
        CategoryCreateModel createModel,
        HttpRequest request)
    {
        var createdModel = categoryService.Create(createModel);
        return TypedResults.Created(request.Host + request.Path + "/" + createdModel.Id, createdModel);
    }

    private static Results<NoContent, NotFound> Update(
        ICategoryService categoryService,
        CategoryUpdateModel updateModel)
    {
        return categoryService.Update(updateModel) ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static Results<NoContent, NotFound> Delete(
        ICategoryService categoryService,
        int id)
    {
        return categoryService.Delete(id) ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}