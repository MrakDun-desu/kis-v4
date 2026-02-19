using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Enums;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class ContainerService(
        KisDbContext dbContext,
        TimeProvider timeProvider,
        UserService userService
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly UserService _userService = userService;

    public async Task<ContainerReadAllResponse> ReadAllAsync(ContainerReadAllRequest req, CancellationToken token = default) {
        var query = _dbContext.Containers
            .Include(c => c.Store)
            .Include(c => c.Pipe)
            .Include(c => c.Template)
            .ThenInclude(ct => ct!.StoreItem)
            .AsQueryable();

        if (req.StoreId is { } storeId) {
            query = query.Where(c => c.StoreId == storeId);
        }

        if (req.TemplateId is { } templateId) {
            query = query.Where(c => c.TemplateId == templateId);
        }

        if (req.PipeId is { } pipeId) {
            query = query.Where(c => c.PipeId == pipeId);
        }

        if (!req.IncludeUnusable) {
            query = query.Where(c => c.State == ContainerState.New || c.State == ContainerState.Opened);
        }

        return await query.PaginateAsync(
                req,
                c => new ContainerListModel {
                    Id = c.Id,
                    Amount = c.Amount,
                    State = c.State,
                    Pipe = c.Pipe.ToModel(),
                    Store = c.Store!.ToModel(),
                    Template = c.Template!.ToModel()
                },
                (data, meta) => new ContainerReadAllResponse { Data = data, Meta = meta },
                c => c.Id,
                token: token
            );
    }

    public async Task<ContainerReadResponse?> ReadAsync(int id, CancellationToken token = default) {
        return await _dbContext.Containers
            .Include(c => c.Template)
            .ThenInclude(ct => ct!.StoreItem)
            .Include(c => c.Pipe)
            .Include(c => c.Store)
            .Include(c => c.ContainerChanges)
            .ThenInclude(cc => cc.User)
            .Select(c => new ContainerReadResponse {
                Id = c.Id,
                Amount = c.Amount,
                State = c.State,
                Template = c.Template!.ToModel(),
                Store = c.Store!.ToModel(),
                Pipe = c.Pipe.ToModel(),
                ContainerChanges = c.ContainerChanges.Select(cc => cc.ToModel())
            })
            .FirstOrDefaultAsync(c => c.Id == id, token);
    }

    public async Task<ContainerCreateResponse> CreateAsync(ContainerCreateRequest req, int userId, CancellationToken token = default) {
        var reqTime = _timeProvider.GetUtcNow();
        var template = await _dbContext.ContainerTemplates
            .Include(t => t.StoreItem)
            .FirstAsync(t => t.Id == req.TemplateId, token);
        var store = await _dbContext.Stores.FindAsync(req.StoreId, token);
        var user = await _userService.GetOrCreateAsync(userId, token);

        var containers = Enumerable.Range(0, req.Amount).Select(_ => new Container {
            Template = template,
            TemplateId = req.TemplateId,
            Store = store,
            StoreId = req.StoreId,
            Amount = template.Amount
        });

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);
        _dbContext.Containers.AddRange(containers);

        try {
            await StoreTransactionService.CreateInternalAsync(
                    new StoreTransactionCreateRequest {
                        Reason = StoreTransactionReason.AddingToStore,
                        StoreId = req.StoreId,
                        StoreTransactionItems = [
                            new StoreTransactionItemCreateRequest {
                            Cost = req.Cost,
                            ItemAmount = req.Amount * template.Amount,
                            StoreItemId = template.StoreItemId
                        }
                        ]
                    },
                userId,
                reqTime,
                _dbContext,
                token: token
            );

            await _dbContext.SaveChangesAsync(token);
            await transaction.CommitAsync();
        } catch {
            await transaction.RollbackAsync(token);
            throw;
        }

        var data = containers.Select(c => new ContainerListModel {
            Id = c.Id,
            Amount = c.Amount,
            State = c.State,
            Store = c.Store!.ToModel(),
            Pipe = c.Pipe.ToModel(),
            Template = c.Template!.ToModel()
        }).ToArray();

        return new ContainerCreateResponse { Data = data };
    }

    public async Task<ContainerUpdateResponse?> UpdateAsync(int id, ContainerUpdateRequest req, int userId, CancellationToken token) {
        var reqTime = _timeProvider.GetUtcNow();
        var user = await _userService.GetOrCreateAsync(userId, token);

        var entity = await _dbContext.Containers
            .FirstOrDefaultAsync(c => c.Id == id, token);

        if (entity is null) {
            return null;
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        entity.StoreId = req.StoreId;
        entity.PipeId = req.PipeId;

        _dbContext.Containers.Update(entity);
        try {
            if (entity.StoreId != req.StoreId) {
                await StoreTransactionService.CreateInternalAsync(
                        new StoreTransactionCreateRequest {
                            Reason = StoreTransactionReason.ChangingStores,
                            StoreId = req.StoreId,
                            SourceStoreId = entity.StoreId,
                            StoreTransactionItems = [
                                new StoreTransactionItemCreateRequest {
                                Cost = 0,
                                ItemAmount = entity.Amount,
                                StoreItemId = entity.Template!.StoreItemId
                            }
                            ]
                        },
                    userId,
                    reqTime,
                    _dbContext,
                    token: token
                );
            }
            await _dbContext.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        } catch {
            await transaction.RollbackAsync(token);
            throw;
        }

        return await _dbContext.Containers
            .AsNoTracking()
            .Include(c => c.Store)
            .Include(c => c.Pipe)
            .Include(c => c.Template)
            .ThenInclude(ct => ct!.StoreItem)
            .Select(c =>
                new ContainerUpdateResponse {
                    Id = entity.Id,
                    Amount = entity.Amount,
                    State = entity.State,
                    Pipe = entity.Pipe.ToModel(),
                    Store = entity.Store!.ToModel(),
                    Template = entity.Template!.ToModel()
                }
            )
            .FirstAsync(c => c.Id == id, token);
    }
}
