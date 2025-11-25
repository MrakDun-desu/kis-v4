using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class CashBoxCreateValidator : AbstractValidator<CashBoxCreateRequest> {
    public CashBoxCreateValidator() {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CashBoxUpdateValidator : AbstractValidator<CashBoxUpdateRequest> {
    public CashBoxUpdateValidator() {
        RuleFor(x => x.Name).NotEmpty();
    }
}
