using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class CompositeReadAllValidator : AbstractValidator<CompositeAmountReadAllRequest> {
    private readonly KisDbContext _dbContext;

    public CompositeReadAllValidator(KisDbContext dbContext) {
        _dbContext = dbContext;
        Include(new PagedRequestValidator());

        RuleFor(x => x.StoreId)
            .MustAsync(StoreExists)
            .WithMessage("Specified store must exist");
    }

    private async Task<bool> StoreExists(int storeId, CancellationToken token = default) =>
        await _dbContext.Stores.FindAsync(storeId, token) is not null;
}

