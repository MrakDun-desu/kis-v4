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

        var includeUnusable = req.IncludeUnusable ?? false;

        if (!includeUnusable) {
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

    public async Task<ContainerOperatorReadResponse?> ReadOperatorAsync(
        int id,
        CancellationToken token = default
    ) {
        return await _dbContext.Containers
            .Include(c => c.Template)
            .ThenInclude(ct => ct!.StoreItem)
            .Select(c => new ContainerOperatorReadResponse {
                Id = c.Id,
                Amount = c.Amount,
                State = c.State,
                Template = c.Template!.ToModel(),
                SaleItems = _dbContext.SaleItems
                    .Include(si => si.Compositions)
                    .ThenInclude(c => c.StoreItem)
                    .Where(si => si.Compositions.Any(comp => comp.StoreItemId == c.Template!.StoreItemId))
                    .Select(si => new SaleItemOperatorModel {
                        Id = si.Id,
                        Name = si.Name,
                        Image = si.Image,
                        CurrentCost = Math.Round(si.Compositions
                            .Sum(c => c.Amount * c.StoreItem!.CurrentCost)
                            * (si.MarginPercent * 0.01m + 1m) + si.MarginStatic, 2),
                        AmountInStore = _dbContext.CompositeAmounts.First(
                            ca => ca.CompositeId == si.Id && ca.StoreId == c.StoreId
                        ).Amount
                    }).ToArray()
            })
            .FirstOrDefaultAsync(c => c.Id == id, token);
    }

    public async Task<ContainerCreateResponse> CreateAsync(ContainerCreateRequest req, int userId, CancellationToken token = default) {
        var reqTime = _timeProvider.GetUtcNow();
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);

        try {
            // create a transaction to add all the required container items to the store
            var template = await _dbContext.ContainerTemplates
                .Include(t => t.StoreItem)
                .FirstAsync(t => t.Id == req.TemplateId, token);
            await StoreTransactionService.CreateInternalAsync(
                    new StoreTransactionCreateRequest {
                        Note = "Automatic store transaction triggered by container creation",
                        UpdateCosts = req.UpdateCosts,
                        Reason = TransactionReason.AddingToStore,
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

            var store = await _dbContext.Stores.FindAsync(req.StoreId, token);
            var user = await _userService.GetAsync(userId, token);

            var containers = Enumerable.Range(0, req.Amount).Select(_ => new Container {
                TemplateId = req.TemplateId,
                StoreId = req.StoreId,
                Amount = template.Amount
            })
            .ToArray();
            _dbContext.Containers.AddRange(containers);
            await _dbContext.SaveChangesAsync(token);

            var data = containers.Select(c => new ContainerListModel {
                Id = c.Id,
                Amount = c.Amount,
                State = c.State,
                Store = store!.ToModel(),
                Pipe = null,
                Template = template.ToModel()
            }).ToArray();

            await transaction.CommitAsync(token);
            return new ContainerCreateResponse { Data = data };
        } catch {
            await transaction.RollbackAsync(token);
            throw;
        }
    }

    public async Task<ContainerUpdateResponse?> UpdateAsync(
        ContainerUpdateRequest req,
        int userId,
        CancellationToken token
    ) {
        var id = req.Id;
        var model = req.Model;
        var reqTime = _timeProvider.GetUtcNow();
        var user = await _userService.GetAsync(userId, token);

        var entity = await _dbContext.Containers
            .FirstOrDefaultAsync(c => c.Id == id, token);

        if (entity is null) {
            return null;
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try {
            entity.StoreId = model.StoreId;
            entity.PipeId = model.PipeId;

            _dbContext.Containers.Update(entity);
            await _dbContext.SaveChangesAsync(token);

            if (entity.StoreId != model.StoreId) {
                await StoreTransactionService.CreateInternalAsync(
                        new StoreTransactionCreateRequest {
                            Reason = TransactionReason.ChangingStores,
                            StoreId = model.StoreId,
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

            var output = await _dbContext.Containers
                .Include(c => c.Store)
                .Include(c => c.Pipe)
                .Include(c => c.Template)
                .ThenInclude(ct => ct!.StoreItem)
                .Select(c =>
                    new ContainerUpdateResponse {
                        Id = c.Id,
                        Amount = c.Amount,
                        State = c.State,
                        Pipe = c.Pipe.ToModel(),
                        Store = c.Store!.ToModel(),
                        Template = c.Template!.ToModel()
                    }
                )
                .FirstAsync(c => c.Id == id, token);

            await transaction.CommitAsync(token);
            return output;
        } catch {
            await transaction.RollbackAsync(token);
            throw;
        }
    }

}
