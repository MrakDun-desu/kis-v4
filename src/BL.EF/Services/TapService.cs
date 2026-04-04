using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Enums;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class TapService(
    KisDbContext dbContext,
    TimeProvider timeProvider,
    UserService userService
) : IScopedService {
    private readonly KisDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly UserService _userService = userService;

    public async Task<TapReadAllResponse> ReadAllAsync(
        CancellationToken token = default
    ) {
        var data = await _dbContext.Taps
            .Include(t => t.Store)
            .Select(p => new TapListModel {
                Id = p.Id,
                Name = p.Name,
                ContainerId = p.ContainerId,
                Store = p.Store!.ToModel()
            })
            .ToArrayAsync(token);

        return new TapReadAllResponse { Data = data };
    }

    public async Task<TapCreateResponse> CreateAsync(
        TapCreateRequest req,
        CancellationToken token = default
    ) {
        var entity = new Tap {
            Name = req.Name,
            StoreId = req.StoreId
        };

        _dbContext.Taps.Add(entity);
        await _dbContext.SaveChangesAsync(token);

        return new TapCreateResponse {
            Id = entity.Id,
            Name = entity.Name,
            ContainerId = null,
            Store = entity.Store!.ToModel()
        };
    }

    public async Task<TapUpdateResponse?> UpdateAsync(
        TapUpdateRequest req,
        string userId,
        CancellationToken token = default
    ) {
        var id = req.Id;
        var model = req.Model;
        var reqTime = _timeProvider.GetUtcNow();
        var entity = await _dbContext.Taps
            .Include(t => t.Store)
            .FirstAsync(t => t.Id == req.Id, token);

        if (entity is null) {
            return null;
        }

        await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(token);

        try {
            entity.Name = model.Name;
            entity.ContainerId = model.ContainerId;

            if (model.ContainerId is { } newContainerId) {
                var newContainer = await _dbContext.Containers
                    .Include(c => c.Template)
                    .FirstAsync(c => c.Id == newContainerId, token);

                var oldStoreId = newContainer.StoreId;
                newContainer.StoreId = entity.StoreId;
                if (oldStoreId != newContainer.StoreId) {
                    await StoreTransactionService.CreateInternalAsync(
                        new StoreTransactionCreateRequest {
                            Reason = TransactionReason.ChangingStores,
                            StoreId = newContainer.StoreId,
                            SourceStoreId = oldStoreId,
                            Note = "Přesunutí kegu",
                            StoreTransactionItems = [
                                new StoreTransactionItemCreateRequest {
                                    Cost = 0,
                                    Amount = newContainer.Amount,
                                    StoreItemId = newContainer.Template!.StoreItemId,
                                }
                            ]
                        },
                        userId,
                        reqTime,
                        _dbContext,
                        token: token
                    );
                }

                if (newContainer!.State == ContainerState.New) {
                    var user = await _userService.GetAsync(userId, token);
                    newContainer.State = ContainerState.Opened;

                    _dbContext.Containers.Update(newContainer);
                    _dbContext.ContainerChanges.Add(new() {
                        ContainerId = newContainerId,
                        NewAmount = newContainer.Amount,
                        NewState = ContainerState.Opened,
                        Timestamp = reqTime,
                        UserId = user.Id
                    });

                }
            }

            _dbContext.Taps.Update(entity);
            await _dbContext.SaveChangesAsync(token);
            await dbTransaction.CommitAsync(token);

            return new TapUpdateResponse {
                Id = entity.Id,
                Name = entity.Name,
                ContainerId = entity.ContainerId,
                Store = entity.Store!.ToModel()
            };
        } catch {
            await dbTransaction.RollbackAsync(token);
            throw;
        }

    }

    public async Task<TapReadResponse?> ReadAsync(
        TapReadRequest req,
        CancellationToken token = default
    ) {
        var containers = await _dbContext.Containers
            .Include(c => c.Store)
            .Include(c => c.Tap)
            .Include(c => c.Template)
            .ThenInclude(ct => ct!.StoreItem)
            .ToArrayAsync(token);

        return await _dbContext.Taps
            .Select(p => new TapReadResponse {
                Id = p.Id,
                Name = p.Name,
                ContainerId = p.ContainerId,
                Store = p.Store!.ToModel(),
                Containers = containers.Select(c => new ContainerListModel {
                    Id = c.Id,
                    Amount = c.Amount,
                    State = c.State,
                    Template = c.Template!.ToModel(),
                    Store = c.Store!.ToModel(),
                    Tap = c.Tap.ToModel()
                }),
            })
            .FirstOrDefaultAsync(p => p.Id == req.Id, token);
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken token
    ) {
        var deleted = await _dbContext.Taps
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync(token);

        return deleted == 1;
    }
}
