using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class StoreItemService(
        KisDbContext dbContext,
        UserService userService,
        TimeProvider timeProvider
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;
    private readonly UserService _userService = userService;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<StoreItemReadAllResponse> ReadAllAsync(
            StoreItemReadAllRequest req,
            CancellationToken token = default
            ) {
        var query = _dbContext.StoreItems.AsQueryable();

        if (req.Name is { } name) {
            query = query.Where(si => si.Name.ToLower().Contains(name));
        }

        if (req.IsContainerItem is { } isContainerItem) {
            query = query.Where(si => si.IsContainerItem == isContainerItem);
        }

        if (req.CategoryId is { } categoryId) {
            query = query.Where(si => si.Categories.Any(c => c.Id == categoryId));
        }

        return await query.PaginateAsync(
                req,
                si => new StoreItemListModel {
                    Id = si.Id,
                    IsContainerItem = si.IsContainerItem,
                    Name = si.Name,
                    CurrentCost = si.CurrentCost,
                    UnitName = si.UnitName
                },
                (data, meta) => new StoreItemReadAllResponse { Data = data, Meta = meta },
                si => si.Id,
                token: token
            );
    }

    public async Task<StoreItemReadResponse?> ReadAsync(
            int id,
            CancellationToken token = default
            ) {
        return await _dbContext.StoreItems
            .Include(si => si.Categories)
            .Include(si => si.Costs)
            .ThenInclude(si => si.User)
            .Select(si => new StoreItemReadResponse {
                Id = si.Id,
                Name = si.Name,
                UnitName = si.UnitName,
                IsContainerItem = si.IsContainerItem,
                CurrentCost = si.CurrentCost,
                Categories = si.Categories.Select(c => c.ToModel()),
            })
            .FirstOrDefaultAsync(si => si.Id == id, token);
    }

    public async Task<StoreItemCreateResponse> CreateAsync(
            StoreItemCreateRequest req,
            int userId,
            CancellationToken token = default
            ) {
        var reqTime = _timeProvider.GetUtcNow();
        var user = await _userService.GetAsync(userId, token);

        var categories = await _dbContext.Categories
            .Where(c => req.CategoryIds.Contains(c.Id))
            .ToArrayAsync(token);

        var entity = new StoreItem {
            Name = req.Name,
            UnitName = req.UnitName,
            IsContainerItem = req.IsContainerItem,
            Costs = [
                new Cost {
                    Amount = req.InitialCost,
                    Description = "Initial cost",
                    Timestamp = reqTime,
                    UserId = user.Id
                }
            ],
            CurrentCost = req.InitialCost,
            Categories = categories
        };

        _dbContext.StoreItems.Add(entity);
        // add amounts for the new entity
        _dbContext.StoreItemAmounts.AddRange(
            _dbContext.Stores.Select(s => new StoreItemAmount {
                StoreItem = entity,
                StoreId = s.Id
            })
        );
        await _dbContext.SaveChangesAsync(token);

        return new StoreItemCreateResponse {
            Id = entity.Id,
            Name = entity.Name,
            UnitName = entity.UnitName,
            IsContainerItem = entity.IsContainerItem,
            CurrentCost = entity.CurrentCost,
            Categories = entity.Categories.Select(c => c.ToModel())
        };
    }

    public async Task<StoreItemUpdateResponse?> UpdateAsync(
            StoreItemUpdateRequest sth,
            CancellationToken token = default
            ) {
        var id = sth.Id;
        var model = sth.Model;
        var entity = await _dbContext.StoreItems
            .Include(si => si.Categories)
            .Include(si => si.Costs)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(si => si.Id == id, token);

        if (entity is null) {
            return null;
        }

        var categories = await _dbContext.Categories
            .Where(c => model.CategoryIds.Contains(c.Id))
            .ToArrayAsync(token);

        entity.Name = model.Name;
        entity.UnitName = model.UnitName;
        entity.Categories.Clear();
        foreach (var category in categories) {
            entity.Categories.Add(category);
        }

        _dbContext.StoreItems.Update(entity);
        await _dbContext.SaveChangesAsync(token);

        return new StoreItemUpdateResponse {
            Id = entity.Id,
            Name = entity.Name,
            UnitName = entity.UnitName,
            IsContainerItem = entity.IsContainerItem,
            CurrentCost = entity.CurrentCost,
            Categories = entity.Categories.Select(c => c.ToModel())
        };
    }

    public async Task<bool> DeleteAsync(
        StoreItemDeleteRequest req,
        CancellationToken token = default
    ) {
        var id = req.Id;
        await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(token);
        try {
            var changedAmount = await _dbContext.StoreItems
                .Where(si => si.Id == id)
                .ExecuteUpdateAsync(props => props.SetProperty(si => si.Hidden, true), token);

            if (changedAmount == 0) {
                return false;
            }

            await _dbContext.Compositions
                .IgnoreQueryFilters()
                .Where(c => c.StoreItemId == id)
                .ExecuteDeleteAsync(token);

            await dbTransaction.CommitAsync(token);
            return true;
        } catch {
            await dbTransaction.RollbackAsync(token);
            throw;
        }
    }
}
