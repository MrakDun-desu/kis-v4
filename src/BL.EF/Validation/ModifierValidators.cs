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
            .InclusiveBetween(-ValidationConstants.MaxAllowedCost, ValidationConstants.MaxAllowedCost)
            .OverridePropertyName(ValidationMessages.MarginStaticPropName)
            .WithMessage(ValidationMessages.MarginOutOfRangeMessage);
        RuleFor(x => x.MarginPercent)
            .InclusiveBetween(-ValidationConstants.MaxMarginPercent, ValidationConstants.MaxMarginPercent)
            .OverridePropertyName(ValidationMessages.MarginPercentPropName)
            .WithMessage(ValidationMessages.MarginOutOfRangeMessage);
        RuleFor(x => x.PrestigeAmount)
            .InclusiveBetween(-ValidationConstants.MaxAllowedCost, ValidationConstants.MaxAllowedCost)
            .OverridePropertyName(ValidationMessages.PrestigePropName)
            .WithMessage(ValidationMessages.PrestigeOutOfRangeMessage);
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
            .InclusiveBetween(-ValidationConstants.MaxAllowedCost, ValidationConstants.MaxAllowedCost)
            .OverridePropertyName(ValidationMessages.MarginStaticPropName)
            .WithMessage(ValidationMessages.MarginOutOfRangeMessage);
        RuleFor(x => x.Model.MarginPercent)
            .InclusiveBetween(-ValidationConstants.MaxMarginPercent, ValidationConstants.MaxMarginPercent)
            .OverridePropertyName(ValidationMessages.MarginPercentPropName)
            .WithMessage(ValidationMessages.MarginOutOfRangeMessage);
        RuleFor(x => x.Model.PrestigeAmount)
            .InclusiveBetween(-ValidationConstants.MaxAllowedCost, ValidationConstants.MaxAllowedCost)
            .OverridePropertyName(ValidationMessages.PrestigePropName)
            .WithMessage(ValidationMessages.PrestigeOutOfRangeMessage);
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
