using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class CompositionReadAllValidator : AbstractValidator<CompositionReadAllRequest> {
    private readonly KisDbContext _dbContext;

    public CompositionReadAllValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.CompositeId)
            .MustAsync(CompositeExists)
            .WithMessage("Specified composite must exist");
    }

    private async Task<bool> CompositeExists(int compositeId, CancellationToken token = default) =>
        await _dbContext.Composites.FindAsync(compositeId, token) is not null;
}

public class CompositionPutValidator : AbstractValidator<CompositionPutRequest> {
    private readonly KisDbContext _dbContext;

    public CompositionPutValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.CompositeId)
            .MustAsync(CompositeExists)
            .WithMessage("Specified composite must exist");

        RuleFor(x => x.StoreItemId)
            .MustAsync(StoreItemExists)
            .WithMessage("Specified store item must exist");
    }

    private async Task<bool> CompositeExists(int compositeId, CancellationToken token = default) =>
        await _dbContext.Composites.FindAsync(compositeId, token) is not null;

    private async Task<bool> StoreItemExists(int storeItemId, CancellationToken token = default) =>
        await _dbContext.StoreItems.FindAsync(storeItemId, token) is not null;
}
