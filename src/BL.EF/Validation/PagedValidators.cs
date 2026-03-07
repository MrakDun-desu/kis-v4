using FluentValidation;
using KisV4.Common.ModelWrappers;

namespace KisV4.BL.EF.Validation;

public class PagedRequestValidator : AbstractValidator<PagedRequest> {
    public PagedRequestValidator() {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .OverridePropertyName(ValidationMessages.PagePropName)
            .WithMessage(ValidationMessages.PageOutOfRangeMessage);
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, ValidationConstants.MaxPageSize)
            .OverridePropertyName(ValidationMessages.PageSizePropName)
            .WithMessage(ValidationMessages.PageSizeOutOfRangeMessage);
    }
}
