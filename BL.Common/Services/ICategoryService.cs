using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICategoryService {
    CategoryListModel Create(CategoryCreateModel createModel);
    List<CategoryListModel> ReadAll();
    OneOf<CategoryListModel, NotFound> Update(int id, CategoryCreateModel updateModel);
    void Delete(int id);
}
