using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICategoryService
{
    public CategoryListModel Create(CategoryCreateModel createModel);
    public List<CategoryListModel> ReadAll();
    public OneOf<Success, NotFound> Update(int id, CategoryCreateModel updateModel);
    public OneOf<Success, NotFound> Delete(int id);
}