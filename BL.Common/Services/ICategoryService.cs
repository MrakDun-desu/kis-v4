using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface ICategoryService
{
    public int Create(CategoryCreateModel createModel);
    public List<CategoryReadAllModel> ReadAll();
    public bool Update(int id, CategoryUpdateModel updateModel);
    public bool Delete(int id);
}