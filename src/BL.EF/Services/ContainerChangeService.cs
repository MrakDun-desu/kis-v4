using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class ContainerChangeService(
        KisDbContext dbContext,
        TimeProvider timeProvider,
        UserService userService
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly UserService _userService = userService;

    public async Task<ContainerChangeReadAllResponse> ReadAll(ContainerChangeReadAllRequest req, CancellationToken token = default) {
        var data = await _dbContext.ContainerChanges
            .Where(cc => cc.ContainerId == req.ContainerId)
            .Include(cc => cc.User)
            .Select(cc => new ContainerChangeModel {
                ContainerId = cc.ContainerId,
                User = cc.User.ToModel()!,
                NewState = cc.NewState,
                Timestamp = cc.Timestamp
            })
            .ToArrayAsync(token);

        return new ContainerChangeReadAllResponse { Data = data };
    }

    public async Task<ContainerChangeCreateResponse> Create(ContainerChangeCreateRequest req, int userId, CancellationToken token = default) {
        var reqTime = _timeProvider.GetUtcNow();
        var user = await _userService.GetOrCreateAsync(userId, token);
        var entity = new ContainerChange {
            ContainerId = req.ContainerId,
            NewState = req.NewState,
            Timestamp = reqTime,
            UserId = user.Id,
            NewAmount = req.NewAmount
        };
        _dbContext.ContainerChanges.Add(entity);

        var container = (await _dbContext.Containers.FindAsync(req.ContainerId, token))!;
        container.Amount = req.NewAmount;
        container.State = req.NewState;

        _dbContext.Update(container);
        await _dbContext.SaveChangesAsync(token);

        return new ContainerChangeCreateResponse {
            ContainerId = entity.ContainerId,
            NewState = entity.NewState,
            Timestamp = entity.Timestamp,
            User = user
        };
    }
}
