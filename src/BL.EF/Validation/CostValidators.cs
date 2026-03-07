using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class CostsCreateValidator : AbstractValidator<CostCreateRequest> {
    public CostsCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreItemId)
            .MustAsync(helper.IdentifyExistingStoreItem)
            .OverridePropertyName(ValidationMessages.StoreItemIdPropName)
            .WithMessage(ValidationMessages.StoreItemIdNotValidMessage);
        RuleFor(x => x.Description)
            .MaximumLength(ValidationConstants.MaxDescriptionLength)
            .OverridePropertyName(ValidationMessages.DescriptionPropName)
            .WithMessage(ValidationMessages.DescriptionTooLongMessage);
        RuleFor(x => x.Amount)
            .InclusiveBetween(0, ValidationConstants.MaxAllowedCost)
            .OverridePropertyName(ValidationMessages.CostPropName)
            .OverridePropertyName(ValidationMessages.AmountOutOfRangeMessage);
    }
}
