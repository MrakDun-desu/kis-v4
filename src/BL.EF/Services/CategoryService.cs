using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Services;

public class CategoryService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public CategoryReadAllResponse ReadAll() {
        var data = _dbContext.Categories.Select(c => new CategoryModel {
            Id = c.Id,
            Name = c.Name
        });
        return new CategoryReadAllResponse { Data = data };
    }

    public CategoryCreateResponse Create(CategoryCreateRequest req) {
        var entity = new Category { Name = req.Name };

        _dbContext.Categories.Add(entity);
        _dbContext.SaveChanges();

        return new CategoryCreateResponse {
            Id = entity.Id,
            Name = entity.Name,
        };
    }

    public bool Update(int id, CategoryUpdateRequest req) {
        var entity = _dbContext.Categories.Find(id);

        if (entity is null) {
            return false;
        }

        entity.Name = req.Name;
        _dbContext.Categories.Update(entity);
        _dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id) {
        var entity = _dbContext.Categories.Find(id);
        if (entity is null) {
            return false;
        }

        _dbContext.Categories.Remove(entity);
        _dbContext.SaveChanges();

        return true;
    }
}
