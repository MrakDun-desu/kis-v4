using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class StoreTransactionItemCreateValidator : AbstractValidator<StoreTransactionItemCreateRequest> {
    public StoreTransactionItemCreateValidator() {
        RuleFor(x => x.Cost)
            .InclusiveBetween(0, ValidationConstants.MaxAllowedCost)
            .OverridePropertyName(ValidationMessages.CostPropName)
            .WithMessage(ValidationMessages.CostOutOfRangeMessage);
        RuleFor(x => x.Amount)
            .InclusiveBetween(0, ValidationConstants.MaxTransactionAmount)
            .OverridePropertyName(ValidationMessages.AmountPropName)
            .WithMessage(ValidationMessages.AmountOutOfRangeMessage);
        // Not validating store items here because that would mean hitting database multiple times
        // during store transaction creation. Other validation is handled in the StoreTransactionCreateValidator
    }
}
