using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class StoreTransactionItemCreateValidator : AbstractValidator<StoreTransactionItemCreateRequest> {
    public StoreTransactionItemCreateValidator() {
        RuleFor(x => x.Cost)
            .LessThan(ValidationConstants.MaxAllowedCost)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.ItemAmount)
            .LessThan(ValidationConstants.MaxTransactionAmount)
            .GreaterThan(0);
        // Not validating store items here because that would mean hitting database multiple times
        // during store transaction creation. Other validation is handled in the StoreTransactionCreateValidator
    }
}
