using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class CompositionReadAllValidator : AbstractValidator<CompositionReadAllRequest> {
    public CompositionReadAllValidator(ValidationHelper helper) {
        RuleFor(x => x.CompositeId)
            .MustAsync(helper.IdentifyExistingComposite)
            .WithMessage("Specified composite must exist");
    }
}

public class CompositionPutValidator : AbstractValidator<CompositionPutRequest> {
    public CompositionPutValidator(ValidationHelper helper) {
        RuleFor(x => x.CompositeId)
            .MustAsync(helper.IdentifyExistingComposite)
            .WithMessage("Specified composite must exist");

        RuleFor(x => x.StoreItemId)
            .MustAsync(helper.IdentifyExistingStoreItem)
            .WithMessage("Specified store item must exist");
    }
}
