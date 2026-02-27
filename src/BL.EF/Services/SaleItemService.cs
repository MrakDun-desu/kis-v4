using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class SaleItemService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public async Task<SaleItemReadAllResponse> ReadAllAsync(
            SaleItemReadAllRequest req,
            CancellationToken token = default
            ) {
        var query = _dbContext.SaleItems.AsQueryable();

        if (req.Name is { } name) {
            query = query.Where(si => si.Name.ToLowerInvariant().Contains(name));
        }

        if (req.CategoryId is { } categoryId) {
            query = query.Where(si => si.Categories.Any(c => c.Id == categoryId));
        }

        return await query.PaginateAsync(
                req,
                si => new SaleItemListModel {
                    Id = si.Id,
                    Name = si.Name,
                    Image = si.Image,
                    MarginPercent = si.MarginPercent,
                    MarginStatic = si.MarginStatic,
                    PrestigeAmount = si.PrestigeAmount,
                    PrintType = si.PrintType
                },
                (data, meta) => new SaleItemReadAllResponse { Data = data, Meta = meta },
                si => si.Id,
                token: token
            );
    }

    public async Task<SaleItemReadResponse?> ReadAsync(
            int id,
            CancellationToken token = default
            ) {
        return await _dbContext.SaleItems
            .Include(si => si.Categories)
            .Include(si => si.ApplicableModifiers)
            .AsSplitQuery()
            .Select(si => new SaleItemReadResponse {
                Id = si.Id,
                Name = si.Name,
                Image = si.Image,
                MarginPercent = si.MarginPercent,
                MarginStatic = si.MarginStatic,
                PrestigeAmount = si.PrestigeAmount,
                PrintType = si.PrintType,
                Categories = si.Categories.Select(c => c.ToModel()),
                ApplicableModifiers = si.ApplicableModifiers.Select(m => m.ToModel())
            })
            .FirstOrDefaultAsync(si => si.Id == id, token);
    }

    public async Task<SaleItemCreateResponse> CreateAsync(
            SaleItemCreateRequest req,
            CancellationToken token = default
            ) {

        var categories = await _dbContext.Categories
            .Where(c => req.CategoryIds.Contains(c.Id))
            .ToArrayAsync(token);

        var modifiers = await _dbContext.Modifiers
            .Where(m => req.ModifierIds.Contains(m.Id))
            .ToArrayAsync(token);

        var entity = new SaleItem {
            Name = req.Name,
            Image = req.Image,
            MarginPercent = req.MarginPercent,
            MarginStatic = req.MarginStatic,
            PrestigeAmount = req.PrestigeAmount,
            PrintType = req.PrintType,
            Categories = categories,
            ApplicableModifiers = modifiers
        };

        _dbContext.SaleItems.Add(entity);
        // add amounts for the new entity
        _dbContext.CompositeAmounts.AddRange(
            _dbContext.Stores.Select(s => new CompositeAmount {
                Composite = entity,
                StoreId = s.Id
            })
        );
        await _dbContext.SaveChangesAsync(token);

        return new SaleItemCreateResponse {
            Id = entity.Id,
            Name = entity.Name,
            Image = entity.Image,
            MarginPercent = entity.MarginPercent,
            MarginStatic = entity.MarginStatic,
            PrestigeAmount = entity.PrestigeAmount,
            PrintType = entity.PrintType,
            ApplicableModifiers = entity.ApplicableModifiers.Select(c => c.ToModel()),
            Categories = entity.Categories.Select(c => c.ToModel())
        };
    }

    public async Task<SaleItemUpdateResponse?> UpdateAsync(
            int id,
            SaleItemUpdateRequest req,
            CancellationToken token = default
            ) {
        var entity = await _dbContext.SaleItems
            .Include(si => si.Categories)
            .Include(si => si.ApplicableModifiers)
            .AsSplitQuery()
            .FirstOrDefaultAsync(si => si.Id == id, token);

        if (entity is null) {
            return null;
        }

        var categories = await _dbContext.Categories
            .Where(c => req.CategoryIds.Contains(c.Id))
            .ToArrayAsync(token);

        var modifiers = await _dbContext.Modifiers
            .Where(c => req.ModifierIds.Contains(c.Id))
            .ToArrayAsync(token);

        entity.Name = req.Name;
        entity.Image = req.Image;
        entity.MarginPercent = req.MarginPercent;
        entity.MarginStatic = req.MarginStatic;
        entity.PrestigeAmount = req.PrestigeAmount;
        entity.PrintType = req.PrintType;
        entity.Categories.Clear();
        foreach (var category in categories) {
            entity.Categories.Add(category);
        }
        entity.ApplicableModifiers.Clear();
        foreach (var modifier in modifiers) {
            entity.ApplicableModifiers.Add(modifier);
        }

        _dbContext.SaleItems.Update(entity);
        await _dbContext.SaveChangesAsync(token);

        return new SaleItemUpdateResponse {
            Id = entity.Id,
            Name = entity.Name,
            Image = entity.Image,
            MarginPercent = entity.MarginPercent,
            MarginStatic = entity.MarginStatic,
            PrestigeAmount = entity.PrestigeAmount,
            PrintType = entity.PrintType,
            Categories = entity.Categories.Select(c => c.ToModel()),
            ApplicableModifiers = entity.ApplicableModifiers.Select(m => m.ToModel())
        };
    }

    public async Task<bool> DeleteAsync(
            int id,
            CancellationToken token = default
            ) {
        var changedAmount = await _dbContext.SaleItems
            .Where(si => si.Id == id)
            .ExecuteUpdateAsync(props => props.SetProperty(si => si.Hidden, true), token);

        return changedAmount > 0;
    }
}
