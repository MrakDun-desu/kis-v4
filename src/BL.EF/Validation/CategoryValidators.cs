using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateRequest> {
    public CategoryCreateValidator() {
        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
    }
}

public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateRequest> {
    public CategoryUpdateValidator() {
        RuleFor(x => x.Model.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
    }
}
