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
}
