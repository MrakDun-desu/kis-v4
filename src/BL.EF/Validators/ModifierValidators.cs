using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class ModifiersReadAllValidator : AbstractValidator<ModifierReadAllRequest> {
    public ModifiersReadAllValidator(ValidationHelper helper) {
        RuleFor(x => x.CategoryId)
            .MustAsync(helper.BeNullOrIdentifyExistingCategory)
            .WithMessage("CategoryId must either be null or identify an existing category");
    }
}

public class ModifierCreateRequestValidator : AbstractValidator<ModifierCreateRequest> {
    public ModifierCreateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
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
            .WithMessage("All category IDs must identify existing categories");
        RuleFor(x => x.TargetIds)
            .MustAsync(helper.AllIdentifyExistingSaleItems)
            .WithMessage("All modification target IDs must identify existing sale items");
    }
}

public class ModifierUpdateRequestValidator : AbstractValidator<ModifierUpdateRequest> {
    public ModifierUpdateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
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
            .WithMessage("All category IDs must identify existing categories");
        RuleFor(x => x.TargetIds)
            .MustAsync(helper.AllIdentifyExistingSaleItems)
            .WithMessage("All modification target IDs must identify existing sale items");
    }
}
