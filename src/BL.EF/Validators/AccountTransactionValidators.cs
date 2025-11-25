using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class AccountTransactionReadAllValidator : AbstractValidator<AccountTransactionReadAllRequest> {
    private readonly KisDbContext _dbContext;

    public AccountTransactionReadAllValidator(KisDbContext dbContext) {
        _dbContext = dbContext;
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
            .Must(AccountExists)
            .WithMessage("Account must exist");
    }

    private bool AccountExists(int accountId) =>
        _dbContext.Accounts.Find(accountId) is not null;
}
