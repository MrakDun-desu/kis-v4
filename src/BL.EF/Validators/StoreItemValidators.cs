using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class StoreItemsReadAllValidator : AbstractValidator<StoreItemReadAllRequest> {
    public StoreItemsReadAllValidator(ValidationHelper helper) {
        RuleFor(x => x.CategoryId)
            .MustAsync(helper.BeNullOrIdentifyExistingCategory)
            .WithMessage("CategoryId must either be null or identify an existing category");
    }
}

public class StoreItemCreateRequestValidator : AbstractValidator<StoreItemCreateRequest> {
    public StoreItemCreateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.CategoryIds)
            .MustAsync(helper.AllIdentifyExistingCategories)
            .WithMessage("All category IDs must identify existing categories");
        RuleFor(x => x.InitialCost)
            .GreaterThan(0);
    }
}

public class StoreItemUpdateRequestValidator : AbstractValidator<StoreItemUpdateRequest> {
    public StoreItemUpdateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.CategoryIds)
            .MustAsync(helper.AllIdentifyExistingCategories)
            .WithMessage("All category IDs must identify existing categories");
    }
}
