using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class ContainerTemplateService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public async Task<ContainerTemplateReadAllResponse> ReadAllAsync(
            ContainerTemplateReadAllRequest req,
            CancellationToken token = default) {

        var query = _dbContext.ContainerTemplates
            .Include(ct => ct.StoreItem)
            .AsQueryable();
        if (req.Name is not null) {
            query = query.Where(x => x.Name.Contains(req.Name));
        }

        if (req.StoreItemId is { } storeItemId) {
            query = query.Where(x => x.StoreItemId == storeItemId);
        }

        return await query.PaginateAsync(
                req,
                ct => new ContainerTemplateModel {
                    Name = ct.Name,
                    Amount = ct.Amount,
                    Id = ct.Id,
                    StoreItem = ct.StoreItem!.ToModel()
                },
                (data, meta) => new ContainerTemplateReadAllResponse { Data = data, Meta = meta },
                ct => ct.Id,
                token: token
                );
    }

    public async Task<ContainerTemplateCreateResponse> CreateAsync(
            ContainerTemplateCreateRequest req,
            CancellationToken token = default
            ) {
        var storeItem = await _dbContext.StoreItems.FindAsync(req.StoreItemId, token);

        var entity = new ContainerTemplate {
            Name = req.Name,
            Amount = req.Amount,
            StoreItemId = req.StoreItemId
        };

        _dbContext.ContainerTemplates.Add(entity);

        await _dbContext.SaveChangesAsync(token);

        return new ContainerTemplateCreateResponse {
            Id = entity.Id,
            Name = entity.Name,
            Amount = entity.Amount,
            StoreItem = storeItem!.ToModel()
        };
    }

    public async Task<ContainerTemplateUpdateResponse?> UpdateAsync(
            int id,
            ContainerTemplateUpdateRequest req,
            CancellationToken token = default
            ) {
        var storeItem = await _dbContext.StoreItems.FindAsync(req.StoreItemId, token);

        var entity = await _dbContext.ContainerTemplates.FindAsync(id, token);

        if (entity is null) {
            return null;
        }

        entity.Amount = req.Amount;
        entity.Name = req.Name;
        entity.StoreItemId = req.StoreItemId;
        entity.Deleted = false;

        _dbContext.Update(entity);

        await _dbContext.SaveChangesAsync(token);

        return new ContainerTemplateUpdateResponse {
            Id = entity.Id,
            Name = entity.Name,
            Amount = entity.Amount,
            StoreItem = storeItem!.ToModel()
        };
    }

    public async Task<bool> DeleteAsync(
            int id,
            CancellationToken token = default
            ) {
        var entity = await _dbContext.ContainerTemplates.FindAsync(id, token);

        if (entity is null) {
            return false;
        }

        entity.Deleted = true;
        _dbContext.Update(entity);

        await _dbContext.SaveChangesAsync(token);

        return true;
    }
}
