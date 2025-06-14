using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Categories {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("categories");
        group.MapGet(string.Empty, ReadAll);
        group.MapPost(string.Empty, Create);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static List<CategoryListModel> ReadAll(ICategoryService categoryService) {
        return categoryService.ReadAll();
    }

    private static Created<CategoryListModel> Create(
        ICategoryService categoryService,
        CategoryCreateModel createModel,
        HttpRequest request) {
        var createdModel = categoryService.Create(createModel);
        return TypedResults.Created(request.Host + request.Path + "/" + createdModel.Id, createdModel);
    }

    private static Results<NoContent, NotFound> Update(
        ICategoryService categoryService,
        CategoryCreateModel updateModel,
        int id) {
        return categoryService.Update(id, updateModel)
            .Match<Results<NoContent, NotFound>>(
                _ => TypedResults.NoContent(),
                _ => TypedResults.NotFound()
            );
    }

    private static NoContent Delete(
        ICategoryService categoryService,
        int id) {
        categoryService.Delete(id);
        return TypedResults.NoContent();
    }
}
