using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class SaleTransactionItemCreateValidator : AbstractValidator<SaleTransactionItemCreateRequest> {
    public SaleTransactionItemCreateValidator() {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .OverridePropertyName(ValidationMessages.AmountPropName)
            .WithMessage(ValidationMessages.AmountTooLowMessage);
        RuleForEach(x => x.Modifications)
            .SetValidator(new ModificationCreateValidator());
    }
}
