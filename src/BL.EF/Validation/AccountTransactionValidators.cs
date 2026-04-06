using FluentValidation;
using KisV4.Common.Enums;
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

public class AccountTransactionCreateValidator : AbstractValidator<AccountTransactionCreateRequest> {
    public AccountTransactionCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.Type)
            .Must(x => x is AccountTransactionType.Deposit or
                    AccountTransactionType.StockTaking or
                    AccountTransactionType.Withdrawal or
                    AccountTransactionType.Transfer)
            .OverridePropertyName(ValidationMessages.TypePropName)
            .WithMessage(ValidationMessages.AccountTransactionTypeNotValidMessage);
        RuleFor(x => x)
            .Must(x => x.Type != AccountTransactionType.Transfer || x.TargetAccountId is not null)
            .WithMessage(ValidationMessages.AccountTransactionTypeAndTargetAccountInvalidMessage);
        RuleFor(x => x.TargetAccountId)
            .MustAsync(helper.BeNullOrIdentifyExistingAccount)
            .OverridePropertyName(ValidationMessages.TargetAccountIdPropName)
            .WithMessage(ValidationMessages.TargetAccountIdNotValidMessage);
        RuleFor(x => x.AccountId)
            .MustAsync(helper.IdentifyExistingAccount)
            .OverridePropertyName(ValidationMessages.AccountIdPropName)
            .WithMessage(ValidationMessages.AccountIdNotValidMessage);
    }
}
