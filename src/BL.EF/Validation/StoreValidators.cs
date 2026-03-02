using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class StoreCreateValidator : AbstractValidator<StoreCreateRequest> {
    public StoreCreateValidator() {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
    }
}

public class StoreUpdateValidator : AbstractValidator<StoreUpdateRequest> {
    public StoreUpdateValidator() {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
    }
}
