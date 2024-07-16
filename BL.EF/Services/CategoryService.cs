using KisV4.BL.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class CategoryService(KisDbContext dbContext, Mapper mapper) : ICategoryService, IScopedService {

    public int Create(CategoryCreateModel createModel) {
        var entity = mapper.ToEntity(createModel);
        var insertedEntity = dbContext.ProductCategories.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public List<CategoryModel> ReadAll() {
        return mapper.ToModels(dbContext.ProductCategories).ToList();
    }

    public bool Update(int id, CategoryUpdateModel updateModel) {
        var entity = dbContext.ProductCategories.Find(id);
        if (entity is null)
            return false;

        var changed = false;

        if (updateModel.Name is not null) {
            entity.Name = updateModel.Name;
            changed = true;
        }

        if (!changed) {
            return true;
        }

        dbContext.ProductCategories.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id) {
        var entity = dbContext.ProductCategories.Find(id);
        if (entity is null) {
            return true;
        }

        dbContext.ProductCategories.Remove(entity);
        dbContext.SaveChanges();

        return true;
    }
}
