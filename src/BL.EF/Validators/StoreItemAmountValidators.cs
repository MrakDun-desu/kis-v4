using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class StoreItemAmountReadAllValidator : AbstractValidator<StoreItemAmountReadAllRequest> {
    public StoreItemAmountReadAllValidator(ValidationHelper helper) {
        Include(new PagedRequestValidator());

        RuleFor(x => x.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .WithMessage("Specified store must exist");
    }
}

