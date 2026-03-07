using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class ModifiersReadAllValidator : AbstractValidator<ModifierReadAllRequest> {
    public ModifiersReadAllValidator(ValidationHelper helper) {
        RuleFor(x => x.CategoryId)
            .MustAsync(helper.BeNullOrIdentifyExistingCategory)
            .OverridePropertyName(ValidationMessages.CategoryIdPropName)
            .WithMessage(ValidationMessages.CategoryIdNotValidMessage);
    }
}

public class ModifierCreateRequestValidator : AbstractValidator<ModifierCreateRequest> {
    public ModifierCreateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
        RuleFor(x => x.MarginStatic)
            .GreaterThan(-ValidationConstants.MaxAllowedCost)
            .LessThan(ValidationConstants.MaxAllowedCost);
        RuleFor(x => x.MarginPercent)
            .GreaterThan(-ValidationConstants.MaxMarginPercent)
            .LessThan(ValidationConstants.MaxMarginPercent);
        RuleFor(x => x.PrestigeAmount)
            .GreaterThan(-ValidationConstants.MaxAllowedCost)
            .LessThan(ValidationConstants.MaxAllowedCost);
        RuleFor(x => x.CategoryIds)
            .MustAsync(helper.AllIdentifyExistingCategories)
            .OverridePropertyName(ValidationMessages.CategoryIdsPropName)
            .WithMessage(ValidationMessages.CategoryIdsNotValidMessage);
        RuleFor(x => x.TargetIds)
            .MustAsync(helper.AllIdentifyExistingSaleItems)
            .OverridePropertyName(ValidationMessages.TargetIdsPropName)
            .WithMessage(ValidationMessages.TargetIdsNotValidMessage);
    }
}

public class ModifierUpdateRequestValidator : AbstractValidator<ModifierUpdateRequest> {
    public ModifierUpdateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.Model.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
        RuleFor(x => x.Model.MarginStatic)
            .GreaterThan(-ValidationConstants.MaxAllowedCost)
            .LessThan(ValidationConstants.MaxAllowedCost);
        RuleFor(x => x.Model.MarginPercent)
            .GreaterThan(-ValidationConstants.MaxMarginPercent)
            .LessThan(ValidationConstants.MaxMarginPercent);
        RuleFor(x => x.Model.PrestigeAmount)
            .GreaterThan(-ValidationConstants.MaxAllowedCost)
            .LessThan(ValidationConstants.MaxAllowedCost);
        RuleFor(x => x.Model.CategoryIds)
            .MustAsync(helper.AllIdentifyExistingCategories)
            .OverridePropertyName(ValidationMessages.CategoryIdsPropName)
            .WithMessage(ValidationMessages.CategoryIdsNotValidMessage);
        RuleFor(x => x.Model.TargetIds)
            .MustAsync(helper.AllIdentifyExistingSaleItems)
            .OverridePropertyName(ValidationMessages.TargetIdsPropName)
            .WithMessage(ValidationMessages.TargetIdsNotValidMessage);
    }
}
