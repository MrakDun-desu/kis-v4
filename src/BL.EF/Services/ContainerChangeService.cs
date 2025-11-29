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

    public ContainerChangeReadAllResponse ReadAll(ContainerChangeReadAllRequest req) {
        var data = _dbContext.ContainerChanges
            .Where(cc => cc.ContainerId == req.ContainerId)
            .Include(cc => cc.User)
            .Select(cc => new ContainerChangeModel {
                ContainerId = cc.ContainerId,
                User = new UserListModel {
                    Id = cc.User!.Id
                },
                NewState = cc.NewState,
                Timestamp = cc.Timestamp
            });

        return new ContainerChangeReadAllResponse { Data = data };
    }

    public ContainerChangeCreateResponse Create(ContainerChangeCreateRequest req, int userId) {
        var reqTime = _timeProvider.GetUtcNow();
        var user = _userService.GetOrCreate(userId);
        var entity = new ContainerChange {
            ContainerId = req.ContainerId,
            NewState = req.NewState,
            Timestamp = reqTime,
            UserId = user.Id,
            NewAmount = req.NewAmount
        };
        _dbContext.ContainerChanges.Add(entity);

        var container = _dbContext.Containers.Find(req.ContainerId)!;
        container.Amount = req.NewAmount;
        container.State = req.NewState;
        _dbContext.Update(container);

        _dbContext.SaveChanges();

        return new ContainerChangeCreateResponse {
            ContainerId = entity.ContainerId,
            NewState = entity.NewState,
            Timestamp = entity.Timestamp,
            User = new UserListModel {
                Id = user.Id
            }
        };
    }
}
