using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class ModifierService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public async Task<ModifierReadAllResponse> ReadAllAsync(
            ModifierReadAllRequest req,
            CancellationToken token = default
            ) {
        var query = _dbContext.Modifiers.AsQueryable();

        if (req.Name is { } name) {
            query = query.Where(si => si.Name.ToLowerInvariant().Contains(name));
        }

        if (req.CategoryId is { } categoryId) {
            query = query.Where(si => si.Categories.Any(c => c.Id == categoryId));
        }

        if (req.TargetId is { } targetId) {
            query = query.Where(m => m.Targets.Any(si => si.Id == targetId));
        }

        return await query.PaginateAsync(
                req,
                si => new ModifierListModel {
                    Id = si.Id,
                    Name = si.Name,
                    Image = si.Image,
                    MarginPercent = si.MarginPercent,
                    MarginStatic = si.MarginStatic,
                    PrestigeAmount = si.PrestigeAmount,
                },
                (data, meta) => new ModifierReadAllResponse { Data = data, Meta = meta },
                si => si.Id,
                token: token
            );
    }

    public async Task<ModifierReadResponse?> ReadAsync(
            int id,
            CancellationToken token = default
            ) {
        return await _dbContext.Modifiers
            .Include(si => si.Categories)
            .Include(m => m.Targets)
            .Select(m => new ModifierReadResponse {
                Id = m.Id,
                Name = m.Name,
                Image = m.Image,
                MarginPercent = m.MarginPercent,
                MarginStatic = m.MarginStatic,
                PrestigeAmount = m.PrestigeAmount,
                Categories = m.Categories.Select(c => c.ToModel()),
                Targets = m.Targets.Select(si => si.ToModel())
            })
            .FirstOrDefaultAsync(si => si.Id == id, token);
    }

    public async Task<ModifierCreateResponse> CreateAsync(
            ModifierCreateRequest req,
            CancellationToken token = default
            ) {
        var categories = await _dbContext.Categories
            .Where(c => req.CategoryIds.Contains(c.Id))
            .ToArrayAsync(token);

        var targets = await _dbContext.SaleItems
            .Where(si => req.TargetIds.Contains(si.Id))
            .ToArrayAsync(token);

        var entity = new Modifier {
            Name = req.Name,
            Image = req.Image,
            MarginPercent = req.MarginPercent,
            MarginStatic = req.MarginStatic,
            PrestigeAmount = req.PrestigeAmount,
            Categories = categories,
            Targets = targets
        };

        _dbContext.Modifiers.Add(entity);
        // add amounts for the new entity
        _dbContext.CompositeAmounts.AddRange(
            _dbContext.Stores.Select(s => new CompositeAmount {
                Composite = entity,
                StoreId = s.Id
            })
        );
        await _dbContext.SaveChangesAsync(token);

        return new ModifierCreateResponse {
            Id = entity.Id,
            Name = entity.Name,
            Image = entity.Image,
            MarginPercent = entity.MarginPercent,
            MarginStatic = entity.MarginStatic,
            PrestigeAmount = entity.PrestigeAmount,
            Targets = entity.Targets.Select(si => si.ToModel()),
            Categories = entity.Categories.Select(c => c.ToModel())
        };
    }

    public async Task<ModifierUpdateResponse?> UpdateAsync(
            int id,
            ModifierUpdateRequest req,
            CancellationToken token = default
            ) {
        var entity = await _dbContext.Modifiers
            .Include(si => si.Categories)
            .Include(si => si.Targets)
            .AsSplitQuery()
            .FirstOrDefaultAsync(si => si.Id == id, token);

        if (entity is null) {
            return null;
        }

        var categories = await _dbContext.Categories
            .Where(c => req.CategoryIds.Contains(c.Id))
            .ToArrayAsync(token);

        var targets = await _dbContext.SaleItems
            .Where(si => req.TargetIds.Contains(si.Id))
            .ToArrayAsync(token);

        entity.Name = req.Name;
        entity.Image = req.Image;
        entity.MarginPercent = req.MarginPercent;
        entity.MarginStatic = req.MarginStatic;
        entity.PrestigeAmount = req.PrestigeAmount;
        entity.Categories.Clear();
        foreach (var category in categories) {
            entity.Categories.Add(category);
        }
        entity.Targets.Clear();
        foreach (var target in targets) {
            entity.Targets.Add(target);
        }

        _dbContext.Modifiers.Update(entity);
        await _dbContext.SaveChangesAsync(token);

        return new ModifierUpdateResponse {
            Id = entity.Id,
            Name = entity.Name,
            Image = entity.Image,
            MarginPercent = entity.MarginPercent,
            MarginStatic = entity.MarginStatic,
            PrestigeAmount = entity.PrestigeAmount,
            Categories = entity.Categories.Select(c => c.ToModel()),
            Targets = entity.Targets.Select(si => si.ToModel())
        };
    }

    public async Task<bool> DeleteAsync(
            int id,
            CancellationToken token = default
            ) {
        var changedAmount = await _dbContext.Modifiers
            .Where(si => si.Id == id)
            .ExecuteUpdateAsync(props => props.SetProperty(si => si.Hidden, true), token);

        return changedAmount > 0;
    }
}
