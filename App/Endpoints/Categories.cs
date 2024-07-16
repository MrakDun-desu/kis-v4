using KisV4.BL.Common;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Categories {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("categories");
        group.MapPost(string.Empty, Create);
        group.MapGet(string.Empty, ReadAll);
        group.MapPut("{id:int}", Update);
        group.MapDelete("{id:int}", Delete);
    }

    private static int Create(
        ICategoryService cashBoxService,
        CategoryCreateModel createModel) {
        var createdId = cashBoxService.Create(createModel);
        return createdId;
    }

    private static List<CategoryModel> ReadAll(ICategoryService cashBoxService) {
        return cashBoxService.ReadAll();
    }

    private static Results<Ok, NotFound> Update(
        ICategoryService cashBoxService,
        int id,
        CategoryUpdateModel updateModel) {
        return cashBoxService.Update(id, updateModel) ? TypedResults.Ok() : TypedResults.NotFound();
    }

    private static Results<Ok, NotFound> Delete(
        ICategoryService cashBoxService,
        int id) {
        return cashBoxService.Delete(id) ? TypedResults.Ok() : TypedResults.NotFound();
    }
}
