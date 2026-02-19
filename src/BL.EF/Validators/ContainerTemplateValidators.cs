using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class ContainerTemplateReadAllValidator : AbstractValidator<ContainerTemplateReadAllRequest> {
    private KisDbContext _dbContext { get; init; }

    public ContainerTemplateReadAllValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreItemId)
            .MustAsync(BeNullOrExistingStoreItem)
            .WithMessage("StoreItemId must either be null or identify an existing store item");
    }

    private async Task<bool> BeNullOrExistingStoreItem(int? storeItemId, CancellationToken token = default) =>
        storeItemId switch {
            null => true,
            { } val => await _dbContext.StoreItems.FindAsync(val, token) is not null
        };
}

public class ContainerTemplateCreateValidator : AbstractValidator<ContainerTemplateCreateRequest> {
    private KisDbContext _dbContext { get; init; }

    public ContainerTemplateCreateValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreItemId)
            .MustAsync(StoreItemExists)
            .WithMessage("Specified store item must exist");
    }

    private async Task<bool> StoreItemExists(int storeItemId, CancellationToken token = default) =>
        await _dbContext.StoreItems.FindAsync(storeItemId, token) is not null;
}

public class ContainerTemplateUpdateValidator : AbstractValidator<ContainerTemplateUpdateRequest> {
    private KisDbContext _dbContext { get; init; }

    public ContainerTemplateUpdateValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreItemId)
            .MustAsync(StoreItemExists)
            .WithMessage("Specified store item must exist");
    }

    private async Task<bool> StoreItemExists(int storeItemId, CancellationToken token = default) =>
        await _dbContext.StoreItems.FindAsync(storeItemId, token) is not null;
}
