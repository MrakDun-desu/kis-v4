using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class ContainerTemplateReadAllValidator : AbstractValidator<ContainerTemplateReadAllRequest> {
    private KisDbContext _dbContext { get; init; }

    public ContainerTemplateReadAllValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreItemId)
            .MustAsync(BeNullOrContainerItem)
            .WithMessage("The queried store item must be null or container item");
    }

    private async Task<bool> BeNullOrContainerItem(int? storeItemId, CancellationToken token = default) =>
        storeItemId switch {
            null => true,
            { } id => await _dbContext.StoreItems.FindAsync(id, token)
            switch {
                null => false,
                var val => val.IsContainerItem
            }
        };
}

public class ContainerTemplateCreateValidator : AbstractValidator<ContainerTemplateCreateRequest> {
    private KisDbContext _dbContext { get; init; }

    public ContainerTemplateCreateValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreItemId)
            .MustAsync(BeContainerItem)
            .WithMessage("Specified store item must be an existing container item");
    }

    private async Task<bool> BeContainerItem(int storeItemId, CancellationToken token = default) =>
        await _dbContext.StoreItems.FindAsync(storeItemId, token) switch {
            null => false,
            var val => val.IsContainerItem
        };
}

public class ContainerTemplateUpdateValidator : AbstractValidator<ContainerTemplateUpdateRequest> {
    private KisDbContext _dbContext { get; init; }

    public ContainerTemplateUpdateValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreItemId)
            .MustAsync(BeContainerItem)
            .WithMessage("Specified store item must be an existing container item");
    }

    private async Task<bool> BeContainerItem(int storeItemId, CancellationToken token = default) =>
        await _dbContext.StoreItems.FindAsync(storeItemId, token) switch {
            null => false,
            var val => val.IsContainerItem
        };
}
