using FluentValidation;
using KisV4.Common.ModelWrappers;

namespace KisV4.BL.EF.Validators;

public class PagedRequestValidator : AbstractValidator<PagedRequest> {
    public PagedRequestValidator() {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).LessThan(100);
    }
}
