using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class CategoryService(KisDbContext dbContext) : ICategoryService, IScopedService
{
    public CategoryListModel Create(CategoryCreateModel createModel)
    {
        var entity = createModel.ToEntity();
        var insertedEntity = dbContext.ProductCategories.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.ToModel();
    }

    public List<CategoryListModel> ReadAll()
    {
        return dbContext.ProductCategories.ToList().ToModels();
    }

    public OneOf<CategoryListModel, NotFound> Update(int id, CategoryCreateModel updateModel)
    {
        if (!dbContext.ProductCategories.Any(pc => pc.Id == id)) 
            return new NotFound();

        var entity = updateModel.ToEntity();
        entity.Id = id;

        dbContext.ProductCategories.Update(entity);
        dbContext.SaveChanges();

        return entity.ToModel();
    }

    public void Delete(int id)
    {
        var entity = dbContext.ProductCategories.Find(id);
        if (entity is null)
            return;

        dbContext.ProductCategories.Remove(entity);
        dbContext.SaveChanges();
    }
}