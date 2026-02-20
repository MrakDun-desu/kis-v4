using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class CostsReadAllValidator : AbstractValidator<CostReadAllRequest> {
    private readonly KisDbContext _dbContext;

    public CostsReadAllValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreItemId)
            .MustAsync(StoreItemExists)
            .WithMessage("Specified store item must exist");
    }

    private async Task<bool> StoreItemExists(int storeItemId, CancellationToken token = default) =>
        await _dbContext.StoreItems.FindAsync(storeItemId, token) is not null;
}

public class CostsCreateValidator : AbstractValidator<CostCreateRequest> {
    private readonly KisDbContext _dbContext;

    public CostsCreateValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreItemId)
            .MustAsync(StoreItemExists)
            .WithMessage("Specified store item must exist");
    }

    private async Task<bool> StoreItemExists(int storeItemId, CancellationToken token = default) =>
        await _dbContext.StoreItems.FindAsync(storeItemId, token) is not null;
}
