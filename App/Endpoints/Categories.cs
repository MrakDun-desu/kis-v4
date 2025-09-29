using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Categories {
    public const string ReadAllRouteName = "CategoryReadAll";

    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("categories");
        group.MapGet(string.Empty, ReadAll)
            .WithName(ReadAllRouteName);
        group.MapPost(string.Empty, Create);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static List<CategoryListModel> ReadAll(ICategoryService categoryService) {
        // there should never be so many categories so they would require paging
        // so just returning all of them
        return categoryService.ReadAll();
    }

    private static CreatedAtRoute<CategoryListModel> Create(
        ICategoryService categoryService,
        CategoryCreateModel createModel
    ) {
        var createdModel = categoryService.Create(createModel);
        return TypedResults.CreatedAtRoute(createdModel, ReadAllRouteName);
    }

    private static Results<NoContent, NotFound> Update(
        ICategoryService categoryService,
        CategoryCreateModel updateModel,
        int id
    ) {
        return categoryService.Update(id, updateModel)
            .Match<Results<NoContent, NotFound>>(
                static _ => TypedResults.NoContent(),
                static _ => TypedResults.NotFound()
            );
    }

    private static NoContent Delete(
        ICategoryService categoryService,
        int id
    ) {
        categoryService.Delete(id);
        return TypedResults.NoContent();
    }
}
