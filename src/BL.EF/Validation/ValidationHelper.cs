using KisV4.BL.EF.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Enums;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Validation;

public class ValidationHelper(
        KisDbContext dbContext,
        SaleTransactionRequestState state
        ) : IScopedService {
    private readonly KisDbContext _dbContext = dbContext;
    private readonly SaleTransactionRequestState _state = state;

    internal async Task<bool> IdentifyExistingAccount(int accountId, CancellationToken token = default) =>
        await _dbContext.Accounts.FindAsync(accountId, token) is not null;

    internal async Task<bool> IdentifyExistingComposite(int compositeId, CancellationToken token = default) =>
        await _dbContext.Composites.FindAsync(compositeId, token) is not null;

    internal async Task<bool> IdentifyExistingStoreItem(int storeItemId, CancellationToken token = default) =>
        await _dbContext.StoreItems.FindAsync(storeItemId, token) is not null;

    internal async Task<bool> IdentifyExistingContainer(int containerId, CancellationToken token = default) =>
        await _dbContext.Containers.FindAsync(containerId, token) is not null;

    internal async Task<bool> HaveAmountLowerOrEqualToCurrent(
            ContainerChangeCreateRequest req,
            CancellationToken token = default) {
        var container = await _dbContext.Containers.FindAsync(req.ContainerId, token);
        return container is null || container.Amount >= req.NewAmount;
    }

    internal async Task<bool> BeNullOrIdentifyExistingContainerItem(int? storeItemId, CancellationToken token = default) =>
        storeItemId switch {
            null => true,
            { } id => await _dbContext.StoreItems.FindAsync(id, token)
            switch {
                null => false,
                var val => val.IsContainerItem
            }
        };

    internal async Task<bool> IdentifyExistingContainerItem(int storeItemId, CancellationToken token = default) =>
        await _dbContext.StoreItems.FindAsync(storeItemId, token) switch {
            null => false,
            var val => val.IsContainerItem
        };

    internal async Task<bool> BeNullOrIdentifyExistingStore(int? storeId, CancellationToken token = default) => storeId switch {
        null => true,
        { } val => await _dbContext.Stores.FindAsync(val, token) is not null
    };

    internal async Task<bool> BeNullOrIdentifyExistingTemplate(int? templateId, CancellationToken token = default) => templateId switch {
        null => true,
        { } val => await _dbContext.ContainerTemplates.FindAsync(val, token) is not null
    };

    internal async Task<bool> BeNullOrIdentifyExistingPipe(int? pipeId, CancellationToken token = default) => pipeId switch {
        null => true,
        { } val => await _dbContext.Pipes.FindAsync(val, token) is not null
    };

    internal async Task<bool> IdentifyExistingStore(int storeId, CancellationToken token = default) =>
        await _dbContext.Stores.FindAsync(storeId, token) is not null;

    internal async Task<bool> IdentifyExistingTemplate(int templateId, CancellationToken token = default) =>
        await _dbContext.ContainerTemplates.FindAsync(templateId, token) is not null;

    internal async Task<bool> BeNullOrIdentifyExistingCategory(int? categoryId, CancellationToken token = default) =>
        categoryId switch {
            null => true,
            var val => await _dbContext.Categories.FindAsync(val, token) is not null
        };

    internal async Task<bool> AllIdentifyExistingCategories(int[] categoryIds, CancellationToken token = default) {
        var categoryCount = await _dbContext.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .CountAsync(token);

        return categoryCount == categoryIds.Length;
    }

    internal async Task<bool> NotHaveExistingContainers(ContainerTemplateUpdateRequest command, CancellationToken token = default) {
        var hasContainers = await _dbContext.Containers
            .IgnoreQueryFilters()
            .AnyAsync(c => c.TemplateId == command.Id, token);
        return !hasContainers;
    }

    internal async Task<bool> HaveCorrectStateTransition(
        ContainerChangeCreateRequest request, CancellationToken token) {
        var container = await _dbContext.Containers.FindAsync(request.ContainerId, token);
        if (container is null) {
            return true;
        }

        var currentState = container.State;
        var newState = request.NewState;

        return currentState switch {
            // new containers can change to any state
            ContainerState.New => true,
            // opened containers can't get back to "new" state
            ContainerState.Opened => newState switch {
                ContainerState.New => false,
                _ => true,
            },
            // written off containers and bad containers can't change states anymore
            ContainerState.WrittenOff => newState switch {
                ContainerState.WrittenOff => true,
                _ => false
            },
            ContainerState.Bad => newState switch {
                ContainerState.Bad => true,
                _ => false
            },
            _ => throw new ArgumentOutOfRangeException("Invalid enum value"),
        };
    }

    internal async Task<bool> AllIdentifyExistingModifiers(int[] modifierIds, CancellationToken token) {
        return await _dbContext.Modifiers
            .Where(m => modifierIds.Contains(m.Id))
            .CountAsync(token) == modifierIds.Length;
    }

    internal async Task<bool> AllIdentifyExistingSaleItems(int[] saleItemIds, CancellationToken token) {
        return await _dbContext.SaleItems
            .Where(si => saleItemIds.Contains(si.Id))
            .CountAsync(token) == saleItemIds.Length;
    }

    internal async Task<bool> AllHaveNonContainerStoreItems(
        StoreTransactionItemCreateRequest[] storeTransactionItems,
        CancellationToken token = default
    ) {
        var storeItemIds = storeTransactionItems.Select(sti => sti.StoreItemId);
        return !await _dbContext.StoreItems
            .Where(si => storeItemIds.Contains(si.Id))
            .AnyAsync(si => si.IsContainerItem, token);
    }

    internal async Task<bool> AllHaveExistingStoreItems(
        StoreTransactionItemCreateRequest[] storeTransactionItems,
        CancellationToken token = default
    ) {
        var storeItemIds = storeTransactionItems.Select(sti => sti.StoreItemId);
        return await _dbContext.StoreItems
            .Where(si => storeItemIds.Contains(si.Id))
            .CountAsync(token) == storeTransactionItems.Distinct().Count();
    }

    internal async Task<bool> HaveValidTargets(
        LayoutItemCreateRequest[] layoutItems,
        CancellationToken token = default
    ) {
        var saleItemIds = layoutItems
            .Where(li => li.Type == LayoutItemType.SaleItem)
            .Select(li => li.TargetId)
            .Distinct()
            .ToArray();
        var pipeIds = layoutItems
            .Where(li => li.Type == LayoutItemType.Pipe)
            .Select(li => li.TargetId)
            .Distinct()
            .ToArray();
        var layoutIds = layoutItems
            .Where(li => li.Type == LayoutItemType.Layout)
            .Select(li => li.TargetId)
            .Distinct()
            .ToArray();

        return await _dbContext.SaleItems
                .Where(si => saleItemIds.Contains(si.Id))
                .CountAsync(token) == saleItemIds.Length &&
            await _dbContext.Pipes
                .Where(p => pipeIds.Contains(p.Id))
                .CountAsync(token) == pipeIds.Length &&
            await _dbContext.Layouts
                .Where(l => layoutIds.Contains(l.Id))
                .CountAsync(token) == layoutIds.Length;
    }

    internal async Task<bool> AllContainValidComposites(
        SaleTransactionItemCreateRequest[] saleTransactionItems,
        CancellationToken token
    ) {
        var composites = await SaleTransactionService.TryGetCompositesAsync(
            saleTransactionItems,
            _dbContext,
            _state,
            token
        );

        if (composites is null) {
            return false;
        }

        foreach (var item in saleTransactionItems) {
            if (composites.TryGetValue(item.SaleItemId, out var saleItem)) {
                if (saleItem.Item is not SaleItem) {
                    return false;
                }
            }
            foreach (var modification in item.Modifications) {
                if (composites.TryGetValue(modification.ModifierId, out var modifier)) {
                    if (modifier.Item is not Modifier) {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    internal async Task<bool> ItemAmountsArentNegative(
        SaleTransactionItemCreateRequest[] saleTransactionItems,
        CancellationToken token
    ) {
        var composites = await SaleTransactionService.TryGetCompositesAsync(
            saleTransactionItems,
            _dbContext,
            _state,
            token
        );

        if (composites is null) {
            return true;
        }

        var storeTransactionItems = SaleTransactionService.GetStoreTransactionItems(
            composites,
            saleTransactionItems,
            0 // store is irrelevent, these items aren't going to be created either way
        );

        return storeTransactionItems.Values.All(sti => sti.ItemAmount <= 0);
    }

    internal async Task<bool> PaidAmountIsEnough(
        SaleTransactionItemCreateRequest[] saleTransactionItems,
        decimal paidAmount,
        CancellationToken token
    ) {
        var composites = await SaleTransactionService.TryGetCompositesAsync(
            saleTransactionItems,
            _dbContext,
            _state,
            token
        );

        if (composites is null) {
            return true;
        }

        var totalPrice = saleTransactionItems.Aggregate(0m, (acc, curr) =>
            acc + (composites[curr.SaleItemId].Price +
                curr.Modifications.Sum(m => composites[m.ModifierId].Price * m.Amount)
            ) * curr.Amount
        );

        return paidAmount >= totalPrice;
    }

    internal async Task<bool> PaidAmountIsEnough(
        int openTransactionId,
        decimal paidAmount,
        CancellationToken token
    ) {
        _state.SaleTransactionItems ??= await _dbContext.SaleTransactionItems
                    .Where(sti => sti.SaleTransactionId == openTransactionId)
                    .Include(sti => sti.Modifications)
                    .ThenInclude(m => m.Modifier)
                    .ToArrayAsync(token);

        var saleTransactionItems = _state.SaleTransactionItems;

        var totalPrice = saleTransactionItems.Aggregate(0m, (acc, curr) =>
            acc + (curr.BasePrice + curr.Modifications.Sum(m => m.PriceChange * m.Amount)) * curr.Amount
        );

        return paidAmount >= totalPrice;
    }

    internal async Task<bool> IdentifyExistingCashBox(int cashBoxId, CancellationToken token)
        => await _dbContext.Cashboxes.FindAsync(cashBoxId, token) is not null;

    internal async Task<bool> AllModifiersAreCorrect(
        SaleTransactionItemCreateRequest[] saleTransactionItems,
        CancellationToken token
    ) {
        var modifierIds = saleTransactionItems
            .SelectMany(sti => sti.Modifications.Select(m => m.ModifierId))
            .ToHashSet();
        var modifierTargets = await _dbContext.Modifiers
            .Where(m => modifierIds.Contains(m.Id))
            .Include(m => m.Targets)
            .Select(m => new {
                m.Id,
                TargetIds = m.Targets.Select(si => si.Id).ToArray()
            })
            .ToDictionaryAsync(m => m.Id, m => m.TargetIds, token);

        // this means that some of the modifiers don't exist, handled by different validator
        if (modifierTargets.Count != modifierIds.Count) {
            return true;
        }

        foreach (var item in saleTransactionItems) {
            foreach (var modification in item.Modifications) {
                if (!modifierTargets[modification.ModifierId].Contains(item.SaleItemId)) {
                    return false;
                }
            }
        }
        return true;
    }
}
