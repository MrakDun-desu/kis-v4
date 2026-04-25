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
            query = query.Where(si => si.Name.ToLower().Contains(name));
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
                    SendToFood = si.SendToFood,
                    TriggerTablePicker = si.TriggerTablePicker
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
            .Include(si => si.Compositions)
            .ThenInclude(c => c.StoreItem)
            .AsSplitQuery()
            .Select(si => new SaleItemReadResponse {
                Id = si.Id,
                Name = si.Name,
                Image = si.Image,
                MarginPercent = si.MarginPercent,
                MarginStatic = si.MarginStatic,
                PrestigeAmount = si.PrestigeAmount,
                Categories = si.Categories.Select(c => c.ToModel()),
                ApplicableModifiers = si.ApplicableModifiers.Select(m => m.ToModel()),
                SendToFood = si.SendToFood,
                TriggerTablePicker = si.TriggerTablePicker,
                CurrentCost = Math.Round(si.Compositions
                    .Sum(c => c.Amount * c.StoreItem!.CurrentCost)
                    * (si.MarginPercent * 0.01m + 1m) + si.MarginStatic, 2),
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
            Categories = categories,
            ApplicableModifiers = modifiers,
            SendToFood = req.SendToFood,
            TriggerTablePicker = req.TriggerTablePicker
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
            ApplicableModifiers = entity.ApplicableModifiers.Select(c => c.ToModel()),
            Categories = entity.Categories.Select(c => c.ToModel()),
            SendToFood = entity.SendToFood,
            TriggerTablePicker = entity.TriggerTablePicker,
            CurrentCost = 0m
        };
    }

    public async Task<SaleItemUpdateResponse?> UpdateAsync(
            SaleItemUpdateRequest req,
            CancellationToken token = default
            ) {
        var id = req.Id;
        var model = req.Model;
        var entity = await _dbContext.SaleItems
            .Include(si => si.Categories)
            .Include(si => si.ApplicableModifiers)
            .Include(si => si.Compositions)
            .ThenInclude(c => c.StoreItem)
            .AsSplitQuery()
            .FirstOrDefaultAsync(si => si.Id == id, token);

        if (entity is null) {
            return null;
        }

        var categories = await _dbContext.Categories
            .Where(c => model.CategoryIds.Contains(c.Id))
            .ToArrayAsync(token);

        var modifiers = await _dbContext.Modifiers
            .Where(c => model.ModifierIds.Contains(c.Id))
            .ToArrayAsync(token);

        entity.Name = model.Name;
        entity.Image = model.Image;
        entity.MarginPercent = model.MarginPercent;
        entity.MarginStatic = model.MarginStatic;
        entity.PrestigeAmount = model.PrestigeAmount;
        entity.SendToFood = model.SendToFood;
        entity.TriggerTablePicker = model.TriggerTablePicker;
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
            Categories = entity.Categories.Select(c => c.ToModel()),
            ApplicableModifiers = entity.ApplicableModifiers.Select(m => m.ToModel()),
            SendToFood = entity.SendToFood,
            TriggerTablePicker = entity.TriggerTablePicker,
            CurrentCost = Math.Round(entity.Compositions
                .Sum(c => c.Amount * c.StoreItem!.CurrentCost)
                * (entity.MarginPercent * 0.01m + 1m) + entity.MarginStatic, 2),
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
