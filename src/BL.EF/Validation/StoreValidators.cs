using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class StoreCreateValidator : AbstractValidator<StoreCreateRequest> {
    public StoreCreateValidator() {
        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
    }
}

public class StoreUpdateValidator : AbstractValidator<StoreUpdateRequest> {
    public StoreUpdateValidator() {
        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
    }
}
