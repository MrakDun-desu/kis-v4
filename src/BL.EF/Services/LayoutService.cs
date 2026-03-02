using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Enums;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class LayoutService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public async Task<LayoutReadAllResponse> ReadAllAsync(
        LayoutReadAllRequest req,
        CancellationToken token = default
    ) {
        var query = _dbContext.Layouts.AsQueryable();
        if (req.Name is { } name) {
            query = query.Where(l => l.Name
                .ToLower()
                .Contains(name.ToLower())
            );
        }


        var data = await query.Select(l => new LayoutListModel {
            Id = l.Id,
            Image = l.Image,
            Name = l.Name,
            TopLevel = l.TopLevel
        })
        .ToArrayAsync(token);

        return new LayoutReadAllResponse { Data = data };
    }

    public async Task<LayoutReadResponse?> ReadTopLevelAsync(
        LayoutReadTopLevelRequest cmd,
        CancellationToken token = default
    ) {
        var storeId = cmd.StoreId;
        var layout = await _dbContext.Layouts.FirstOrDefaultAsync(l => l.TopLevel, token);

        if (layout is null) {
            return null;
        }
        var layoutItems = await GetLayoutItemsAsync(layout.Id, storeId, token);

        return new LayoutReadResponse {
            Id = layout.Id,
            Image = layout.Image,
            Name = layout.Name,
            TopLevel = layout.TopLevel,
            LayoutItems = layoutItems
        };
    }

    public async Task<LayoutReadResponse?> ReadAsync(
        LayoutReadRequest cmd,
        CancellationToken token = default
    ) {
        var layout = await _dbContext.Layouts.FindAsync(cmd.Id, token);

        if (layout is null) {
            return null;
        }
        var layoutItems = await GetLayoutItemsAsync(layout.Id, cmd.StoreId, token);

        return new LayoutReadResponse {
            Id = layout.Id,
            Image = layout.Image,
            Name = layout.Name,
            TopLevel = layout.TopLevel,
            LayoutItems = layoutItems
        };
    }

    public async Task<LayoutCreateResponse> CreateAsync(
        LayoutCreateRequest cmd,
        CancellationToken token = default
    ) {
        var req = cmd.Model;
        var entity = new Layout {
            Name = req.Name,
            Image = req.Image,
            TopLevel = req.TopLevel,
            LayoutItems = [.. req.LayoutItems.Select<LayoutItemCreateRequest, LayoutItem>(li => li.Type switch {
                LayoutItemType.Layout => new LayoutLink {
                    TargetId = li.TargetId,
                    X = li.X,
                    Y = li.Y,
                    Type = LayoutItemType.Layout
                },
                LayoutItemType.Pipe => new LayoutPipe {
                    TargetId = li.TargetId,
                    X = li.X,
                    Y = li.Y,
                    Type = LayoutItemType.Pipe
                },
                LayoutItemType.SaleItem => new LayoutSaleItem {
                    TargetId = li.TargetId,
                    X = li.X,
                    Y = li.Y,
                    Type = LayoutItemType.SaleItem
                },
                _ => throw new ArgumentOutOfRangeException()
            })]
        };

        var topLevelLayouts = await _dbContext.Layouts.Where(l => l.TopLevel).ToArrayAsync(token);
        // if there are no top level layouts yet, the first created one is always going to be top-level
        if (topLevelLayouts.Length == 0) {
            entity.TopLevel = true;
        } else if (entity.TopLevel) {
            // if there already is a top-level layout and the new one is supposed to be top-level,
            // it replaces the existing top-level layout. There should always be exactly one
            foreach (var layout in topLevelLayouts) {
                layout.TopLevel = false;
            }
        }

        _dbContext.Layouts.Add(entity);
        await _dbContext.SaveChangesAsync(token);

        var layoutItems = await GetLayoutItemsAsync(entity.Id, cmd.StoreId, token);
        return new LayoutCreateResponse {
            Id = entity.Id,
            Image = entity.Image,
            Name = entity.Name,
            TopLevel = entity.TopLevel,
            LayoutItems = layoutItems
        };
    }

    public async Task<LayoutUpdateResponse?> UpdateAsync(
        LayoutUpdateRequest cmd,
        CancellationToken token = default
    ) {
        var id = cmd.Id;
        var req = cmd.Model;
        var entity = await _dbContext.Layouts
            .Include(l => l.LayoutItems)
            .FirstOrDefaultAsync(l => l.Id == id, token);

        if (entity is null) {
            return null;
        }

        entity.Name = req.Name;
        entity.Image = req.Image;
        // there should always only be one top-level layout
        if (!entity.TopLevel && req.TopLevel) {
            var topLevelLayouts = await _dbContext.Layouts.Where(l => l.TopLevel).ToArrayAsync(token);
            foreach (var topLevelLayout in topLevelLayouts) {
                topLevelLayout.TopLevel = false;
            }
        }
        entity.TopLevel = req.TopLevel;
        entity.LayoutItems = [
            .. req.LayoutItems.Select<LayoutItemCreateRequest, LayoutItem>(li => li.Type switch {
                LayoutItemType.Layout => new LayoutLink {
                    TargetId = li.TargetId,
                    X = li.X,
                    Y = li.Y,
                    Type = LayoutItemType.Layout
                },
                LayoutItemType.Pipe => new LayoutPipe {
                    TargetId = li.TargetId,
                    X = li.X,
                    Y = li.Y,
                    Type = LayoutItemType.Pipe
                },
                LayoutItemType.SaleItem => new LayoutSaleItem {
                    TargetId = li.TargetId,
                    X = li.X,
                    Y = li.Y,
                    Type = LayoutItemType.SaleItem
                },
                _ => throw new ArgumentOutOfRangeException()
            }
        )];
        _dbContext.Layouts.Update(entity);
        await _dbContext.SaveChangesAsync(token);

        var layoutItems = await GetLayoutItemsAsync(id, cmd.StoreId, token);
        return new LayoutUpdateResponse {
            Id = entity.Id,
            Image = entity.Image,
            Name = entity.Name,
            TopLevel = entity.TopLevel,
            LayoutItems = layoutItems
        };
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken token = default
    ) {
        await _dbContext.LayoutItems
            .Where(li => li.LayoutId == id)
            .ExecuteDeleteAsync(token);
        var deletedCount = await _dbContext.Layouts
            .Where(l => l.Id == id)
            .ExecuteDeleteAsync(token);

        return deletedCount > 0;
    }

    private async Task<IEnumerable<LayoutItemModel>> GetLayoutItemsAsync(
        int layoutId,
        int? storeId,
        CancellationToken token = default
    ) {
        var layoutSaleItems = await _dbContext.LayoutSaleItems
            .Where(li => li.LayoutId == layoutId)
            .Include(li => li.Target)
            .ThenInclude(si => si!.Compositions)
            .ThenInclude(c => c.StoreItem)
            .Select(li => new LayoutSaleItemModel {
                Y = li.Y,
                X = li.X,
                Target = new SaleItemOperatorModel {
                    Id = li.TargetId,
                    Image = li.Target!.Image,
                    Name = li.Target.Name,
                    AmountInStore = storeId.HasValue
                        ? _dbContext.CompositeAmounts.First(
                            ca => ca.CompositeId == li.TargetId && ca.StoreId == storeId.Value
                        ).Amount
                        : null,
                    CurrentCost = Math.Round(li.Target.Compositions
                        .Sum(c => c.Amount * c.StoreItem!.CurrentCost) *
                        (li.Target.MarginPercent * 0.01m + 1m) + li.Target.MarginStatic, 2)
                }
            })
            .ToArrayAsync(token);

        var layoutLinks = await _dbContext.LayoutLinks
            .Where(li => li.LayoutId == layoutId)
            .Include(ll => ll.Target)
            .Select(ll => new LayoutLinkModel {
                X = ll.X,
                Y = ll.Y,
                Target = new LayoutListModel {
                    Id = ll.Target!.Id,
                    Image = ll.Target.Image,
                    Name = ll.Target.Name,
                    TopLevel = ll.Target.TopLevel
                }
            })
            .ToArrayAsync(token);

        var layoutPipes = await _dbContext.LayoutPipes
            .Where(li => li.LayoutId == layoutId)
            .Include(ll => ll.Target)
            .Select(lp => new LayoutPipeModel {
                X = lp.X,
                Y = lp.Y,
                Target = new PipeListModel {
                    Id = lp.Target!.Id,
                    Name = lp.Target.Name
                }
            })
            .ToArrayAsync(token);

        return [.. layoutLinks, .. layoutSaleItems, .. layoutPipes];
    }
}
