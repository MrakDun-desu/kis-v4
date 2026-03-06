using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class StoreItemsReadAllValidator : AbstractValidator<StoreItemReadAllRequest> {
    public StoreItemsReadAllValidator(ValidationHelper helper) {
        RuleFor(x => x.CategoryId)
            .MustAsync(helper.BeNullOrIdentifyExistingCategory)
            .WithMessage("CategoryId must either be null or identify an existing category");
    }
}

public class StoreItemCreateValidator : AbstractValidator<StoreItemCreateRequest> {
    public StoreItemCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
        RuleFor(x => x.UnitName)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxUnitNameLength);
        RuleFor(x => x.CategoryIds)
            .MustAsync(helper.AllIdentifyExistingCategories)
            .WithMessage("All category IDs must identify existing categories");
        RuleFor(x => x.InitialCost)
            .GreaterThan(0)
            .LessThan(ValidationConstants.MaxAllowedCost);
    }
}

public class StoreItemUpdateValidator : AbstractValidator<StoreItemUpdateRequest> {
    public StoreItemUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.Model.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
        RuleFor(x => x.Model.UnitName)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxUnitNameLength);
        RuleFor(x => x.Model.CategoryIds)
            .MustAsync(helper.AllIdentifyExistingCategories)
            .WithMessage("All category IDs must identify existing categories");
    }
}

public class StoreItemDeleteValidator : AbstractValidator<StoreItemDeleteRequest> {
    public StoreItemDeleteValidator(ValidationHelper helper) {
        RuleFor(x => x)
            .MustAsync(helper.NotHaveAnyAssociatedContainerTemplates)
            .WithMessage("Can't delete store items with active container templates");
    }
}
