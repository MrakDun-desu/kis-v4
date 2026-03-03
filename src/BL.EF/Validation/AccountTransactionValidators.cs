using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class AccountTransactionReadAllValidator : AbstractValidator<AccountTransactionReadAllRequest> {
    public AccountTransactionReadAllValidator(ValidationHelper helper) {
        Include(new PagedRequestValidator());
        RuleFor(x => x)
            .Must(x =>
                // if either of them is null, no need to check
                x.From is null || x.To is null || x.From < x.To
            )
            .WithMessage("The datetime From must be earlier than the datetime To if both of them are set");

        RuleFor(x => x.AccountId)
            .MustAsync(helper.IdentifyExistingAccount)
            .WithMessage("Account must exist");
    }
}
