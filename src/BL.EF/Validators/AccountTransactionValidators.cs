using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class AccountTransactionReadAllValidator : AbstractValidator<AccountTransactionReadAllRequest> {
    public AccountTransactionReadAllValidator(ValidationHelper helper) {
        Include(new PagedRequestValidator());
        RuleFor(x => x)
            .Must(x => {
                // if either of them is null, no need to check
                if (x.From is null || x.To is null) {
                    return true;
                }
                return x.From < x.To;
            })
            .WithMessage("The datetime From must be earlier than the datetime To");

        RuleFor(x => x.AccountId)
            .MustAsync(helper.IdentifyExistingAccount)
            .WithMessage("Account must exist");
    }
}
