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
        UserService userService,
        StoreTransactionService storeTransactionService
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly UserService _userService = userService;
    private readonly StoreTransactionService _storeTransactionService = storeTransactionService;

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
                    Pipe = c.Pipe == null ? null : new PipeListModel {
                        Id = c.Pipe!.Id,
                        Name = c.Pipe.Name
                    },
                    Store = new StoreListModel {
                        Id = c.Store!.Id,
                        Name = c.Store.Name
                    },
                    Template = new ContainerTemplateModel {
                        Id = c.Template!.Id,
                        Name = c.Template.Name,
                        Amount = c.Template.Amount,
                        StoreItem = new StoreItemListModel {
                            Id = c.Template.StoreItem!.Id,
                            Name = c.Template.StoreItem.Name,
                            CurrentCost = c.Template.StoreItem.CurrentCost,
                            IsContainerItem = c.Template.StoreItem.IsContainerItem,
                            UnitName = c.Template.StoreItem.UnitName,
                        }
                    }
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
                Template = new ContainerTemplateModel {
                    Id = c.Template!.Id,
                    Amount = c.Template.Amount,
                    Name = c.Template.Name,
                    StoreItem = new StoreItemListModel {
                        Id = c.Template.StoreItem!.Id,
                        Name = c.Template.StoreItem.Name,
                        CurrentCost = c.Template.StoreItem.CurrentCost,
                        IsContainerItem = c.Template.StoreItem.IsContainerItem,
                        UnitName = c.Template.StoreItem.UnitName,
                    }
                },
                Store = new StoreListModel {
                    Id = c.Store!.Id,
                    Name = c.Store.Name
                },
                Pipe = c.Pipe == null ? null : new PipeListModel {
                    Id = c.Pipe!.Id,
                    Name = c.Pipe.Name
                },
                ContainerChanges = c.ContainerChanges.Select(cc => new ContainerChangeModel {
                    ContainerId = cc.ContainerId,
                    NewState = cc.NewState,
                    Timestamp = cc.Timestamp,
                    User = new UserListModel {
                        Id = cc.User!.Id
                    }
                })
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

        _dbContext.Database.BeginTransaction();
        _storeTransactionService.CreateInternal(
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
            reqTime
        );

        _dbContext.Containers.AddRange(containers);
        _dbContext.SaveChanges();

        _dbContext.Database.CommitTransaction();

        var data = containers.Select(c => new ContainerListModel {
            Id = c.Id,
            Amount = c.Amount,
            State = c.State,
            Store = new StoreListModel {
                Id = c.Store!.Id,
                Name = c.Store.Name
            },
            Pipe = null,
            Template = new ContainerTemplateModel {
                Id = c.Template!.Id,
                Amount = c.Template.Amount,
                Name = c.Template.Name,
                StoreItem = new StoreItemListModel {
                    Id = c.Template.StoreItem!.Id,
                    Name = c.Template.StoreItem.Name,
                    CurrentCost = c.Template.StoreItem.CurrentCost,
                    IsContainerItem = c.Template.StoreItem.IsContainerItem,
                    UnitName = c.Template.StoreItem.UnitName
                }
            }
        });

        return new ContainerCreateResponse { Data = data };
    }

    public ContainerUpdateResponse? Update(int id, ContainerUpdateRequest req, int userId) {
        var reqTime = _timeProvider.GetUtcNow();
        var user = _userService.GetOrCreate(userId);

        var entity = _dbContext.Containers
            .Include(c => c.Store)
            .Include(c => c.Pipe)
            .Include(c => c.Template)
            .ThenInclude(ct => ct!.StoreItem)
            .FirstOrDefault(c => c.Id == id);

        if (entity is null) {
            return null;
        }

        _dbContext.Database.BeginTransaction();

        if (entity.StoreId != req.StoreId) {
            _storeTransactionService.CreateInternal(
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
                reqTime
            );
        }

        entity.StoreId = req.StoreId;
        entity.PipeId = req.PipeId;

        _dbContext.Containers.Update(entity);
        _dbContext.SaveChanges();

        _dbContext.Database.CommitTransaction();

        return new ContainerUpdateResponse {
            Id = entity.Id,
            Amount = entity.Amount,
            State = entity.State,
            Pipe = entity.Pipe == null ? null : new PipeListModel {
                Id = entity.Pipe.Id,
                Name = entity.Pipe.Name
            },
            Store = new StoreListModel {
                Id = entity.Store!.Id,
                Name = entity.Store.Name
            },
            Template = new ContainerTemplateModel {
                Id = entity.Template!.Id,
                Name = entity.Template.Name,
                Amount = entity.Template.Amount,
                StoreItem = new StoreItemListModel {
                    Id = entity.Template.StoreItem!.Id,
                    Name = entity.Template.StoreItem.Name,
                    CurrentCost = entity.Template.StoreItem.CurrentCost,
                    IsContainerItem = entity.Template.StoreItem.IsContainerItem,
                    UnitName = entity.Template.StoreItem.UnitName,
                }
            }
        };
    }
}
