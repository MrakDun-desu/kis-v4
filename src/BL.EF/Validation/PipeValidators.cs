using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class PipeCreateValidator : AbstractValidator<PipeCreateRequest> {
    public PipeCreateValidator() {
        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
    }
}

public class PipeUpdateValidator : AbstractValidator<PipeUpdateRequest> {
    public PipeUpdateValidator() {
        RuleFor(x => x.Model.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
    }
}
