using KisV4.Common.DependencyInjection;
using KisV4.Common.Enums;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Validators;

public class ValidationHelper(
        KisDbContext dbContext
        ) : IScopedService {
    private readonly KisDbContext _dbContext = dbContext;

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
        if (container is null) {
            return true;
        }

        return container.Amount >= req.NewAmount;
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

    internal async Task<bool> NotHaveExistingContainers(ContainerTemplateUpdateCommand command, CancellationToken token = default) {
        var hasContainers = await _dbContext.Containers.AnyAsync(c => c.TemplateId == command.Id, token);
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
}
