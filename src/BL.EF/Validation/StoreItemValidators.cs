using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class StoreItemsReadAllValidator : AbstractValidator<StoreItemReadAllRequest> {
    public StoreItemsReadAllValidator(ValidationHelper helper) {
        RuleFor(x => x.CategoryId)
            .MustAsync(helper.BeNullOrIdentifyExistingCategory)
            .OverridePropertyName(ValidationMessages.CategoryIdPropName)
            .WithMessage(ValidationMessages.CategoryIdNotValidMessage);
    }
}

public class StoreItemCreateValidator : AbstractValidator<StoreItemCreateRequest> {
    public StoreItemCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
        RuleFor(x => x.UnitName)
            .MaximumLength(ValidationConstants.MaxUnitNameLength)
            .OverridePropertyName(ValidationMessages.UnitNamePropName)
            .WithMessage(ValidationMessages.UnitNameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.UnitNamePropName)
            .WithMessage(ValidationMessages.UnitNameEmptyMessage);
        RuleFor(x => x.CategoryIds)
            .MustAsync(helper.AllIdentifyExistingCategories)
            .OverridePropertyName(ValidationMessages.CategoryIdsPropName)
            .WithMessage(ValidationMessages.CategoryIdsNotValidMessage);
        RuleFor(x => x.InitialCost)
            .GreaterThan(0)
            .LessThan(ValidationConstants.MaxAllowedCost);
    }
}

public class StoreItemUpdateValidator : AbstractValidator<StoreItemUpdateRequest> {
    public StoreItemUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.Model.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
        RuleFor(x => x.Model.UnitName)
            .MaximumLength(ValidationConstants.MaxUnitNameLength)
            .OverridePropertyName(ValidationMessages.UnitNamePropName)
            .WithMessage(ValidationMessages.UnitNameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.UnitNamePropName)
            .WithMessage(ValidationMessages.UnitNameEmptyMessage);
        RuleFor(x => x.Model.CategoryIds)
            .MustAsync(helper.AllIdentifyExistingCategories)
            .OverridePropertyName(ValidationMessages.CategoryIdsPropName)
            .WithMessage(ValidationMessages.CategoryIdsNotValidMessage);
    }
}

public class StoreItemDeleteValidator : AbstractValidator<StoreItemDeleteRequest> {
    public StoreItemDeleteValidator(ValidationHelper helper) {
        RuleFor(x => x)
            .MustAsync(helper.NotHaveAnyAssociatedContainerTemplates)
            .WithMessage(ValidationMessages.StoreItemUsedInContainersMessage);
    }
}
