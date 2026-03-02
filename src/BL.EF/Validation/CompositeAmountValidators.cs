using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class CompositeAmountReadAllValidator : AbstractValidator<CompositeAmountReadAllRequest> {
    public CompositeAmountReadAllValidator(ValidationHelper helper) {
        Include(new PagedRequestValidator());

        RuleFor(x => x.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .WithMessage("Specified store must exist");
    }
}
