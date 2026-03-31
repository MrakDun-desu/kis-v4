using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class PipeService(
    KisDbContext dbContext
) : IScopedService {
    private readonly KisDbContext _dbContext = dbContext;

    public async Task<PipeReadAllResponse> ReadAllAsync(
        CancellationToken token = default
    ) {
        var data = await _dbContext.Pipes
            .Select(p => new PipeListModel {
                Id = p.Id,
                Name = p.Name
            })
            .ToArrayAsync(token);

        return new PipeReadAllResponse { Data = data };
    }

    public async Task<PipeCreateResponse> CreateAsync(
        PipeCreateRequest req,
        CancellationToken token = default
    ) {
        var entity = new Pipe {
            Name = req.Name
        };

        _dbContext.Pipes.Add(entity);
        await _dbContext.SaveChangesAsync(token);

        return new PipeCreateResponse {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public async Task<PipeUpdateResponse?> UpdateAsync(
        PipeUpdateRequest req,
        CancellationToken token = default
    ) {
        var id = req.Id;
        var model = req.Model;
        var entity = await _dbContext.Pipes.FindAsync(id, token);

        if (entity is null) {
            return null;
        }

        entity.Name = model.Name;
        _dbContext.Pipes.Update(entity);
        await _dbContext.SaveChangesAsync(token);

        return new PipeUpdateResponse {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public async Task<PipeReadResponse?> ReadAsync(
        PipeReadRequest req,
        CancellationToken token = default
    ) {
        var containersQuery = _dbContext.Containers
            .Where(c => c.PipeId == req.Id)
            .Include(c => c.Template)
            .ThenInclude(ct => ct!.StoreItem)
            .AsQueryable();
        if (req.StoreId is { } storeId) {
            containersQuery = containersQuery.Where(c => c.StoreId == storeId);
        }
        var containers = await containersQuery.ToArrayAsync(token);

        return await _dbContext.Pipes
            .Select(p => new PipeReadResponse {
                Id = p.Id,
                Name = p.Name,
                Containers = containers.Select(c => new ContainerPipeModel {
                    Id = c.Id,
                    Amount = c.Amount,
                    State = c.State,
                    Template = c.Template!.ToModel(),
                    StoreId = c.StoreId
                }),
            })
            .FirstOrDefaultAsync(p => p.Id == req.Id, token);
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken token
    ) {
        var deleted = await _dbContext.Pipes
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync(token);

        return deleted == 1;
    }
}
