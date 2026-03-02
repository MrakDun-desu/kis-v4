using FluentValidation;
using KisV4.Common.ModelWrappers;

namespace KisV4.BL.EF.Validation;

public class PagedRequestValidator : AbstractValidator<PagedRequest> {
    public PagedRequestValidator() {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).LessThan(ValidationConstants.MaxPageSize);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}
