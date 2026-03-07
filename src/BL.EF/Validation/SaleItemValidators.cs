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
            .GreaterThanOrEqualTo(0)
            .LessThan(ValidationConstants.MaxAllowedCost);
        RuleFor(x => x.MarginPercent)
            .GreaterThanOrEqualTo(0)
            .LessThan(ValidationConstants.MaxMarginPercent);
        RuleFor(x => x.PrestigeAmount)
            .GreaterThanOrEqualTo(0)
            .LessThan(ValidationConstants.MaxAllowedCost);
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
            .GreaterThanOrEqualTo(0)
            .LessThan(ValidationConstants.MaxAllowedCost);
        RuleFor(x => x.Model.MarginPercent)
            .GreaterThanOrEqualTo(0)
            .LessThan(ValidationConstants.MaxMarginPercent);
        RuleFor(x => x.Model.PrestigeAmount)
            .GreaterThanOrEqualTo(0)
            .LessThan(ValidationConstants.MaxAllowedCost);
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
