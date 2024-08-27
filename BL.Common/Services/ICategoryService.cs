using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICategoryService
{
    public CategoryReadAllModel Create(CategoryCreateModel createModel);
    public List<CategoryReadAllModel> ReadAll();
    public OneOf<Success, NotFound> Update(CategoryUpdateModel updateModel);
    public OneOf<Success, NotFound> Delete(int id);
}