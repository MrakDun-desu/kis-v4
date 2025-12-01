using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class CategoryService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public async Task<CategoryReadAllResponse> ReadAllAsync(CancellationToken token = default) {
        var data = await _dbContext.Categories.Select(c => new CategoryModel {
            Id = c.Id,
            Name = c.Name
        }).ToArrayAsync(token);

        return new CategoryReadAllResponse { Data = data };
    }

    public async Task<CategoryCreateResponse> CreateAsync(CategoryCreateRequest req, CancellationToken token = default) {
        var entity = new Category { Name = req.Name };

        _dbContext.Categories.Add(entity);
        await _dbContext.SaveChangesAsync(token);

        return new CategoryCreateResponse {
            Id = entity.Id,
            Name = entity.Name,
        };
    }

    public async Task<bool> UpdateAsync(int id, CategoryUpdateRequest req, CancellationToken token = default) {
        var entity = await _dbContext.Categories.FindAsync(id, token);

        if (entity is null) {
            return false;
        }

        entity.Name = req.Name;
        _dbContext.Categories.Update(entity);
        await _dbContext.SaveChangesAsync(token);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken token = default) {
        var entity = await _dbContext.Categories.FindAsync(id, token);
        if (entity is null) {
            return false;
        }

        _dbContext.Categories.Remove(entity);
        await _dbContext.SaveChangesAsync(token);

        return true;
    }
}
