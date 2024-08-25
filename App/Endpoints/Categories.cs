using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Categories
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("categories");
        group.MapPost(string.Empty, Create);
        group.MapGet(string.Empty, ReadAll);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static int Create(
        ICategoryService categoryService,
        CategoryCreateModel createModel)
    {
        var createdId = categoryService.Create(createModel);
        return createdId;
    }

    private static List<CategoryReadAllModel> ReadAll(ICategoryService categoryService)
    {
        return categoryService.ReadAll();
    }

    private static Results<Ok, NotFound> Update(
        ICategoryService categoryService,
        int id,
        CategoryUpdateModel updateModel)
    {
        return categoryService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> Delete(
        ICategoryService categoryService,
        int id)
    {
        return categoryService.Delete(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}