using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class SaleItemsReadAllValidator : AbstractValidator<SaleItemReadAllRequest> {
    public SaleItemsReadAllValidator(ValidationHelper helper) {
        RuleFor(x => x.CategoryId)
            .MustAsync(helper.BeNullOrIdentifyExistingCategory)
            .WithMessage("CategoryId must either be null or identify an existing category");
    }
}

public class SaleItemCreateRequestValidator : AbstractValidator<SaleItemCreateRequest> {
    public SaleItemCreateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
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
            .WithMessage("All category IDs must identify existing categories");
        RuleFor(x => x.ModifierIds)
            .MustAsync(helper.AllIdentifyExistingModifiers)
            .WithMessage("All modifier IDs must identify existing modifiers");
    }
}

public class SaleItemUpdateRequestValidator : AbstractValidator<SaleItemUpdateRequest> {
    public SaleItemUpdateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
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
            .WithMessage("All category IDs must identify existing categories");
    }
}
