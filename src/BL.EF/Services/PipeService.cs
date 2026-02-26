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
        int id,
        PipeUpdateRequest req,
        CancellationToken token = default
    ) {
        var entity = await _dbContext.Pipes.FindAsync(id, token);

        if (entity is null) {
            return null;
        }

        entity.Name = req.Name;
        _dbContext.Pipes.Update(entity);
        await _dbContext.SaveChangesAsync(token);

        return new PipeUpdateResponse {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public async Task<PipeReadResponse?> ReadAsync(
        int id,
        CancellationToken token = default
    ) {
        var entity = await _dbContext.Pipes
            .Include(p => p.Containers)
            .ThenInclude(c => c.Template)
            .ThenInclude(ct => ct!.StoreItem)
            .FirstAsync(p => p.Id == id, token);

        if (entity is null) {
            return null;
        }

        var containerStoreItems = entity.Containers
            .Select(c => c.Template!.StoreItemId)
            .Distinct()
            .ToArray();

        return new PipeReadResponse {
            Id = entity.Id,
            Name = entity.Name,
            Containers = entity.Containers.Select(c => new ContainerPipeModel {
                Id = c.Id,
                Amount = c.Amount,
                State = c.State,
                Template = c.Template!.ToModel()
            }),
        };
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
