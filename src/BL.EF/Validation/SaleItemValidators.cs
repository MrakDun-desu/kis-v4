using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class SaleItemsReadAllValidator : AbstractValidator<SaleItemReadAllRequest> {
    public SaleItemsReadAllValidator(ValidationHelper helper) {
        RuleFor(x => x.CategoryId)
            .MustAsync(helper.BeNullOrIdentifyExistingCategory)
            .OverridePropertyName(ValidationMessages.CategoryIdPropName)
            .WithMessage(ValidationMessages.CategoryIdNotValidMessage);
    }
}

public class SaleItemCreateRequestValidator : AbstractValidator<SaleItemCreateRequest> {
    public SaleItemCreateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
        RuleFor(x => x.MarginStatic)
            .InclusiveBetween(0, ValidationConstants.MaxAllowedCost)
            .OverridePropertyName(ValidationMessages.MarginStaticPropName)
            .WithMessage(ValidationMessages.MarginOutOfRangeMessage);
        RuleFor(x => x.MarginPercent)
            .InclusiveBetween(0, ValidationConstants.MaxMarginPercent)
            .OverridePropertyName(ValidationMessages.MarginPercentPropName)
            .WithMessage(ValidationMessages.MarginOutOfRangeMessage);
        RuleFor(x => x.PrestigeAmount)
            .InclusiveBetween(0, ValidationConstants.MaxAllowedCost)
            .OverridePropertyName(ValidationMessages.PrestigePropName)
            .WithMessage(ValidationMessages.PrestigeOutOfRangeMessage);
        RuleFor(x => x.CategoryIds)
            .MustAsync(helper.AllIdentifyExistingCategories)
            .OverridePropertyName(ValidationMessages.CategoryIdsPropName)
            .WithMessage(ValidationMessages.CategoryIdsNotValidMessage);
        RuleFor(x => x.ModifierIds)
            .MustAsync(helper.AllIdentifyExistingModifiers)
            .OverridePropertyName(ValidationMessages.ModifierIdsPropName)
            .WithMessage(ValidationMessages.ModifierIdsNotValidMessage);
    }
}

public class SaleItemUpdateRequestValidator : AbstractValidator<SaleItemUpdateRequest> {
    public SaleItemUpdateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.Model.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
        RuleFor(x => x.Model.MarginStatic)
            .InclusiveBetween(0, ValidationConstants.MaxAllowedCost)
            .OverridePropertyName(ValidationMessages.MarginStaticPropName)
            .WithMessage(ValidationMessages.MarginOutOfRangeMessage);
        RuleFor(x => x.Model.MarginPercent)
            .InclusiveBetween(0, ValidationConstants.MaxMarginPercent)
            .OverridePropertyName(ValidationMessages.MarginPercentPropName)
            .WithMessage(ValidationMessages.MarginOutOfRangeMessage);
        RuleFor(x => x.Model.PrestigeAmount)
            .InclusiveBetween(0, ValidationConstants.MaxAllowedCost)
            .OverridePropertyName(ValidationMessages.PrestigePropName)
            .WithMessage(ValidationMessages.PrestigeOutOfRangeMessage);
        RuleFor(x => x.Model.CategoryIds)
            .MustAsync(helper.AllIdentifyExistingCategories)
            .OverridePropertyName(ValidationMessages.CategoryIdsPropName)
            .WithMessage(ValidationMessages.CategoryIdsNotValidMessage);
        RuleFor(x => x.Model.ModifierIds)
            .MustAsync(helper.AllIdentifyExistingModifiers)
            .OverridePropertyName(ValidationMessages.ModifierIdsPropName)
            .WithMessage(ValidationMessages.ModifierIdsNotValidMessage);
    }
}
