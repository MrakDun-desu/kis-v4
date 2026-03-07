using FluentValidation;
using KisV4.Common.Enums;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class LayoutReadValidator : AbstractValidator<LayoutReadRequest> {
    public LayoutReadValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreId)
            .MustAsync(helper.BeNullOrIdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);
    }
}

public class LayoutCreateRequestValidator : AbstractValidator<LayoutCreateRequestModel> {
    public LayoutCreateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
        RuleFor(x => x.LayoutItems)
            .Must(x => x.Select(li => (li.X, li.Y)).Distinct().Count() == x.Count())
            .OverridePropertyName(ValidationMessages.LayoutItemsPropName)
            .WithMessage(ValidationMessages.LayoutItemsNotUniqueMessage);
        RuleFor(x => x.LayoutItems)
            .MustAsync(helper.HaveValidTargets)
            .OverridePropertyName(ValidationMessages.LayoutItemsPropName)
            .WithMessage(ValidationMessages.LayoutItemTargetsNotValidMessage);
        RuleForEach(x => x.LayoutItems)
            .SetValidator(new LayoutItemValidator());
    }
}

public class LayoutCreateValidator : AbstractValidator<LayoutCreateRequest> {
    public LayoutCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreId)
            .MustAsync(helper.BeNullOrIdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);
        RuleFor(x => x.Model)
            .SetValidator(new LayoutCreateRequestValidator(helper));
    }
}

public class LayoutItemValidator : AbstractValidator<LayoutItemCreateRequest> {
    public LayoutItemValidator() {
        RuleFor(x => x.X)
            .InclusiveBetween(0, ValidationConstants.LayoutWidth)
            .WithMessage(ValidationMessages.LayoutItemPositionOutOfRangeMessage);
        RuleFor(x => x.Y)
            .InclusiveBetween(0, ValidationConstants.LayoutHeight)
            .WithMessage(ValidationMessages.LayoutItemPositionOutOfRangeMessage);
    }
}

public class LayoutUpdateRequestValidator : AbstractValidator<LayoutUpdateRequestModel> {
    public LayoutUpdateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
        RuleFor(x => x.LayoutItems)
            .Must(x => x.Select(li => (li.X, li.Y)).Distinct().Count() == x.Count())
            .OverridePropertyName(ValidationMessages.LayoutItemsPropName)
            .WithMessage(ValidationMessages.LayoutItemsNotUniqueMessage);
        RuleFor(x => x.LayoutItems)
            .MustAsync(helper.HaveValidTargets)
            .OverridePropertyName(ValidationMessages.LayoutItemsPropName)
            .WithMessage(ValidationMessages.LayoutItemTargetsNotValidMessage);
        RuleForEach(x => x.LayoutItems)
            .SetValidator(new LayoutItemValidator());
    }
}

public class LayoutUpdateValidator : AbstractValidator<LayoutUpdateRequest> {
    public LayoutUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreId)
            .MustAsync(helper.BeNullOrIdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);
        RuleFor(x => x.Model)
            .SetValidator(new LayoutUpdateRequestValidator(helper));
    }
}

public class LayoutReadTopLevelValidator : AbstractValidator<LayoutReadTopLevelRequest> {
    public LayoutReadTopLevelValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreId)
            .MustAsync(helper.BeNullOrIdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);
    }
}
