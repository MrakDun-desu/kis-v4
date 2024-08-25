using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface ICategoryService
{
    public CategoryReadAllModel Create(CategoryCreateModel createModel);
    public List<CategoryReadAllModel> ReadAll();
    public bool Update(CategoryUpdateModel updateModel);
    public bool Delete(int id);
}