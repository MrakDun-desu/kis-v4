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

    public ContainerReadAllResponse ReadAll(ContainerReadAllRequest req) {
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

        return query.Paginate(
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
                c => c.Id
            );
    }

    public ContainerReadResponse? Read(int id) {
        return _dbContext.Containers
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
            .FirstOrDefault(c => c.Id == id);
    }

    public ContainerCreateResponse Create(ContainerCreateRequest req, int userId) {
        var reqTime = _timeProvider.GetUtcNow();
        var template = _dbContext.ContainerTemplates
            .Include(t => t.StoreItem)
            .First(t => t.Id == req.TemplateId)!;
        var store = _dbContext.Stores.Find(req.StoreId);
        var user = _userService.GetOrCreate(userId);

        var containers = Enumerable.Range(0, req.Amount).Select(_ => new Container {
            Template = template,
            TemplateId = req.TemplateId,
            Store = store,
            StoreId = req.StoreId,
            Amount = template.Amount
        });

        using (var transaction = _dbContext.Database.BeginTransaction()) {

            StoreTransactionService.CreateInternal(
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
                _dbContext
            );

            _dbContext.Containers.AddRange(containers);
            _dbContext.SaveChanges();
        }

        var data = containers.Select(c => new ContainerListModel {
            Id = c.Id,
            Amount = c.Amount,
            State = c.State,
            Store = c.Store!.ToModel(),
            Pipe = c.Pipe.ToModel(),
            Template = c.Template!.ToModel()
        });

        return new ContainerCreateResponse { Data = data };
    }

    public ContainerUpdateResponse? Update(int id, ContainerUpdateRequest req, int userId) {
        var reqTime = _timeProvider.GetUtcNow();
        var user = _userService.GetOrCreate(userId);

        var entity = _dbContext.Containers
            .FirstOrDefault(c => c.Id == id);

        if (entity is null) {
            return null;
        }

        using (var transaction = _dbContext.Database.BeginTransaction()) {

            if (entity.StoreId != req.StoreId) {
                StoreTransactionService.CreateInternal(
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
                    _dbContext
                );
            }

            entity.StoreId = req.StoreId;
            entity.PipeId = req.PipeId;

            _dbContext.Containers.Update(entity);
            _dbContext.SaveChanges();

            _dbContext.Database.CommitTransaction();
        }

        return _dbContext.Containers
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
            .First(c => c.Id == id)
        ;
    }
}
