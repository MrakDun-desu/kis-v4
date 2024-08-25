using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class CategoryService(KisDbContext dbContext) : ICategoryService, IScopedService
{
    public CategoryReadAllModel Create(CategoryCreateModel createModel)
    {
        var entity = createModel.ToEntity();
        var insertedEntity = dbContext.ProductCategories.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.ToModel();
    }

    public List<CategoryReadAllModel> ReadAll()
    {
        return dbContext.ProductCategories.ToList().ToModels();
    }

    public bool Update(CategoryUpdateModel updateModel)
    {
        if (!dbContext.ProductCategories.Any(pc => pc.Id == updateModel.Id))
        {
            return false;
        }

        var entity = updateModel.ToEntity();

        dbContext.ProductCategories.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        var entity = dbContext.ProductCategories.Find(id);
        if (entity is null)
        {
            return false;
        }

        dbContext.ProductCategories.Remove(entity);
        dbContext.SaveChanges();

        return true;
    }
}