using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class CostsCreateValidator : AbstractValidator<CostCreateRequest> {
    public CostsCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreItemId)
            .MustAsync(helper.IdentifyExistingStoreItem)
            .WithMessage("Specified store item must exist");
        RuleFor(x => x.Description)
            .MaximumLength(ValidationConstants.MaxDescriptionLength);
        RuleFor(x => x.Amount)
            .LessThan(ValidationConstants.MaxAllowedCost)
            .GreaterThan(0);
    }
}
