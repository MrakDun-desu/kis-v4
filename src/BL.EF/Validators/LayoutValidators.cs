using FluentValidation;
using KisV4.Common.Enums;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class LayoutReadValidator : AbstractValidator<LayoutReadCommand> {
    public LayoutReadValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreId)
            .MustAsync(helper.BeNullOrIdentifyExistingStore)
            .WithMessage("Store ID must be null or identify existing store");
    }
}

public class LayoutCreateRequestValidator : AbstractValidator<LayoutCreateRequest> {
    public LayoutCreateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
        RuleFor(x => x.LayoutItems)
            .Must(x => x.Select(li => (li.X, li.Y)).Distinct().Count() == x.Count())
            .WithMessage("Layout items must all have unique positions");
        RuleFor(x => x.LayoutItems)
            .MustAsync(helper.HaveValidTargets)
            .WithMessage("Layout items must all have valid target IDs");
        RuleForEach(x => x.LayoutItems)
            .SetValidator(new LayoutItemValidator());
    }
}

public class LayoutCreateValidator : AbstractValidator<LayoutCreateCommand> {
    public LayoutCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreId)
            .MustAsync(helper.BeNullOrIdentifyExistingStore)
            .WithMessage("Store ID must be null or identify existing store");
        RuleFor(x => x.Request)
            .SetValidator(new LayoutCreateRequestValidator(helper));
    }
}

public class LayoutItemValidator : AbstractValidator<LayoutItemCreateRequest> {
    public LayoutItemValidator() {
        RuleFor(x => x.Type)
            .Must(x => x is LayoutItemType.Layout or
                LayoutItemType.Pipe or
                LayoutItemType.SaleItem);
        RuleFor(x => x.X)
            .GreaterThanOrEqualTo(0)
            .LessThan(ValidationConstants.LayoutWidth);
        RuleFor(x => x.Y)
            .GreaterThanOrEqualTo(0)
            .LessThan(ValidationConstants.LayoutHeight);
    }
}

public class LayoutUpdateRequestValidator : AbstractValidator<LayoutUpdateRequest> {
    public LayoutUpdateRequestValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
        RuleFor(x => x.LayoutItems)
            .Must(x => x.Select(li => (li.X, li.Y)).Distinct().Count() == x.Count())
            .WithMessage("Layout items must all have unique positions");
        RuleFor(x => x.LayoutItems)
            .MustAsync(helper.HaveValidTargets)
            .WithMessage("Layout items must all have valid target identifiers");
        RuleForEach(x => x.LayoutItems)
            .SetValidator(new LayoutItemValidator());
    }
}

public class LayoutUpdateValidator : AbstractValidator<LayoutUpdateCommand> {
    public LayoutUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreId)
            .MustAsync(helper.BeNullOrIdentifyExistingStore)
            .WithMessage("Store ID must be null or identify existing store");
        RuleFor(x => x.Request)
            .SetValidator(new LayoutUpdateRequestValidator(helper));
    }
}

public class LayoutReadTopLevelValidator : AbstractValidator<LayoutReadTopLevelCommand> {
    public LayoutReadTopLevelValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreId)
            .MustAsync(helper.BeNullOrIdentifyExistingStore)
            .WithMessage("Store ID must be null or identify existing store");
    }
}
