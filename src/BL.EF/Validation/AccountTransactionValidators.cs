using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class AccountTransactionReadAllValidator : AbstractValidator<AccountTransactionReadAllRequest> {
    public AccountTransactionReadAllValidator(ValidationHelper helper) {
        Include(new PagedRequestValidator());
        RuleFor(x => x.AccountId)
            .MustAsync(helper.IdentifyExistingAccount)
            .OverridePropertyName(ValidationMessages.AccountIdPropName)
            .WithMessage(ValidationMessages.AccountIdNotValidMessage);
    }
}
