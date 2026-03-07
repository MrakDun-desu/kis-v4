using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class CashBoxCreateValidator : AbstractValidator<CashBoxCreateRequest> {
    public CashBoxCreateValidator() {
        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
    }
}

public class CashBoxUpdateValidator : AbstractValidator<CashBoxUpdateRequest> {
    public CashBoxUpdateValidator() {
        RuleFor(x => x.Model.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
    }
}
