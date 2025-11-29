using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class CompositionReadAllValidator : AbstractValidator<CompositionReadAllRequest> {
    private readonly KisDbContext _dbContext;

    public CompositionReadAllValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.CompositeId)
            .Must(CompositeExists)
            .WithMessage("Specified composite must exist");
    }

    private bool CompositeExists(int compositeId) =>
        _dbContext.Composites.Find(compositeId) is not null;
}

public class CompositionPutValidator : AbstractValidator<CompositionPutRequest> {
    private readonly KisDbContext _dbContext;

    public CompositionPutValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.CompositeId)
            .Must(CompositeExists)
            .WithMessage("Specified composite must exist");

        RuleFor(x => x.StoreItemId)
            .Must(StoreItemExists)
            .WithMessage("Specified store item must exist");
    }

    private bool CompositeExists(int compositeId) =>
        _dbContext.Composites.Find(compositeId) is not null;

    private bool StoreItemExists(int storeItemId) =>
        _dbContext.StoreItems.Find(storeItemId) is not null;
}
