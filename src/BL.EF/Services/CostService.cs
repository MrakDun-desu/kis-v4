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

    public async Task<CostCreateResponse> Create(
            CostCreateRequest req,
            int userId,
            CancellationToken token = default
            ) {
        var reqTime = _timeProvider.GetUtcNow();

        var user = await _userService.GetOrCreateAsync(userId, token);

        var storeItem = await _dbContext.StoreItems.FindAsync(req.StoreItemId, token);
        storeItem!.CurrentCost = req.Amount;

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
            StoreItemId = entity.StoreItemId,
            User = entity.User.ToModel()!,
            Timestamp = entity.Timestamp
        };
    }
}
