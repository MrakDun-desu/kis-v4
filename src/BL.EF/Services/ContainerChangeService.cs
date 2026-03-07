using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Enums;
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

    public async Task<ContainerChangeReadAllResponse> ReadAll(
            ContainerChangeReadAllRequest req,
            CancellationToken token = default
        ) {
        var data = await _dbContext.ContainerChanges
            .Where(cc => cc.ContainerId == req.ContainerId)
            .Include(cc => cc.User)
            .Select(cc => new ContainerChangeModel {
                ContainerId = cc.ContainerId,
                User = cc.User.ToModel()!,
                NewState = cc.NewState,
                Timestamp = cc.Timestamp
            })
            .OrderByDescending(cc => cc.Timestamp)
            .ToArrayAsync(token);

        return new ContainerChangeReadAllResponse { Data = data };
    }

    public async Task<ContainerChangeCreateResponse> Create(
        ContainerChangeCreateRequest req,
        int userId,
        CancellationToken token = default
    ) {
        var reqTime = _timeProvider.GetUtcNow();
        var user = await _userService.GetAsync(userId, token);
        var entity = new ContainerChange {
            ContainerId = req.ContainerId,
            NewState = req.NewState,
            Timestamp = reqTime,
            UserId = user.Id,
            NewAmount = req.NewAmount
        };
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);

        try {
            _dbContext.ContainerChanges.Add(entity);

            var container = await _dbContext.Containers
                .Include(c => c.Template)
                .FirstAsync(c => c.Id == req.ContainerId, token);

            // only create a new store transaction if transitioning from new or opened to bad or
            // written off
            // when doing normal container operations, the store transaction should be happening
            // as a part of a sale transaction, so it's unnecessary to change it here
            if (container.State is ContainerState.New or ContainerState.Opened &&
                req.NewState is ContainerState.Bad or ContainerState.WrittenOff) {
                await StoreTransactionService.CreateInternalAsync(
                    new StoreTransactionCreateRequest {
                        Note = "Container write-off",
                        Reason = TransactionReason.WriteOff,
                        StoreId = container.StoreId,
                        StoreTransactionItems = [
                            new StoreTransactionItemCreateRequest {
                                    Cost = 0,
                                    Amount = -container.Amount,
                                    StoreItemId = container.Template!.StoreItemId
                                }
                        ]
                    },
                    userId,
                    reqTime,
                    _dbContext,
                    token: token
                );
                // if writing off or marking container as bad, also remove it from the pipe
                container.PipeId = null;
            }

            container.Amount = req.NewAmount;
            container.State = req.NewState;

            _dbContext.Update(container);
            await _dbContext.SaveChangesAsync(token);


            var output = new ContainerChangeCreateResponse {
                ContainerId = entity.ContainerId,
                NewState = entity.NewState,
                Timestamp = entity.Timestamp,
                User = user
            };

            await transaction.CommitAsync(token);
            return output;
        } catch {
            await transaction.RollbackAsync(token);
            throw;
        }
    }
}
