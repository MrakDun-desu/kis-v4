using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class StoreService(
        KisDbContext dbContext,
        StoreItemAmountService storeItemAmountService,
        ContainerService containerService
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;
    private readonly StoreItemAmountService _storeItemAmountService = storeItemAmountService;
    private readonly ContainerService _containerService = containerService;

    public async Task<StoreReadAllResponse> ReadAllAsync(
            CancellationToken token = default
            ) {
        var data = await _dbContext.Stores
            .Select(s => new StoreListModel {
                Id = s.Id,
                Name = s.Name
            })
            .ToArrayAsync(token);

        return new StoreReadAllResponse { Data = data };
    }

    public async Task<StoreReadResponse?> ReadAsync(
            int id,
            CancellationToken token = default
            ) {
        var store = await _dbContext.Stores.FindAsync(id, token);

        if (store is null) {
            return null;
        }

        var storeItemAmounts = await _storeItemAmountService.ReadAllAsync(new StoreItemAmountReadAllRequest {
            StoreId = id
        }, token);

        var containers = await _containerService.ReadAllAsync(new ContainerReadAllRequest {
            StoreId = id,
            IncludeUnusable = false,
        }, token);

        return new StoreReadResponse {
            Id = store.Id,
            Name = store.Name,
            Containers = containers,
            StoreItemAmounts = storeItemAmounts
        };
    }

    public async Task<StoreCreateResponse> CreateAsync(
            StoreCreateRequest req,
            CancellationToken token = default
            ) {
        var entity = new Store {
            Name = req.Name
        };

        _dbContext.Stores.Add(entity);
        await _dbContext.SaveChangesAsync(token);

        return new StoreCreateResponse {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public async Task<StoreUpdateResponse?> UpdateAsync(
            int id,
            StoreUpdateRequest req,
            CancellationToken token = default
            ) {
        var entity = await _dbContext.Stores.FindAsync(id, token);

        if (entity is null) {
            return null;
        }

        entity.Name = req.Name;
        _dbContext.Stores.Update(entity);
        await _dbContext.SaveChangesAsync(token);

        return new StoreUpdateResponse {
            Name = entity.Name,
            Id = entity.Id
        };
    }

    public async Task<bool> DeleteAsync(
            int id,
            CancellationToken token = default
            ) {
        var entity = await _dbContext.Stores.FindAsync(id, token);

        if (entity is null) {
            return false;
        }

        entity.Deleted = true;
        _dbContext.Stores.Update(entity);
        await _dbContext.SaveChangesAsync(token);

        return true;
    }
}
