using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class StoreCreateValidator : AbstractValidator<StoreCreateRequest> {
    public StoreCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
    }
}

public class StoreUpdateValidator : AbstractValidator<StoreUpdateRequest> {
    public StoreUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
    }
}

