using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICategoryService
{
    public CategoryListModel Create(CategoryCreateModel createModel);
    public List<CategoryListModel> ReadAll();
    public OneOf<CategoryListModel, NotFound> Update(int id, CategoryCreateModel updateModel);
    public void Delete(int id);
}