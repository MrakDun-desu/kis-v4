using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class CompositeReadAllValidator : AbstractValidator<CompositeAmountReadAllRequest> {
    private readonly KisDbContext _dbContext;

    public CompositeReadAllValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreId)
            .Must(StoreExists)
            .WithMessage("Specified store must exist");
    }

    private bool StoreExists(int storeId) =>
        _dbContext.Stores.Find(storeId) is not null;
}

