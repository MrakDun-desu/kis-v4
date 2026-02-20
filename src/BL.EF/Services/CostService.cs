using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class CostService(
        KisDbContext dbContext,
        UserService userService,
        TimeProvider timeProvider
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;
    private readonly UserService _userService = userService;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<CostReadAllResponse> ReadAll(
            CostReadAllRequest req,
            CancellationToken token = default) {
        var data = await _dbContext.Costs
            .Where(c => c.StoreItemId == req.StoreItemId)
            .Include(c => c.User)
            .Include(c => c.StoreItem)
            .Select(c => new CostModel {
                Amount = c.Amount,
                Description = c.Description,
                Timestamp = c.Timestamp,
                User = c.User.ToModel()!,
                StoreItem = c.StoreItem!.ToModel()
            })
            .ToArrayAsync(token);

        return new CostReadAllResponse { Data = data };
    }

    public async Task<CostCreateResponse> Create(
            CostCreateRequest req,
            int userId,
            CancellationToken token = default
            ) {
        var reqTime = _timeProvider.GetUtcNow();

        var storeItem = await _dbContext.StoreItems
            .FindAsync(req.StoreItemId, token);

        var user = await _userService.GetOrCreateAsync(userId, token);

        var entity = new Cost {
            Amount = req.Amount,
            Description = req.Description,
            Timestamp = reqTime,
            StoreItemId = req.StoreItemId,
            UserId = user.Id
        };

        _dbContext.Costs.Add(entity);
        await _dbContext.SaveChangesAsync(token);

        return new CostCreateResponse {
            Amount = entity.Amount,
            Description = entity.Description,
            StoreItem = entity.StoreItem!.ToModel(),
            User = entity.User.ToModel()!,
            Timestamp = entity.Timestamp
        };
    }
}
