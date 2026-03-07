using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class CompositionReadAllValidator : AbstractValidator<CompositionReadAllRequest> {
    public CompositionReadAllValidator(ValidationHelper helper) {
        RuleFor(x => x.CompositeId)
            .MustAsync(helper.IdentifyExistingComposite)
            .OverridePropertyName(ValidationMessages.CompositeIdPropName)
            .WithMessage(ValidationMessages.CompositeIdNotValidMessage);
    }
}

public class CompositionPutValidator : AbstractValidator<CompositionPutRequest> {
    public CompositionPutValidator(ValidationHelper helper) {
        RuleFor(x => x.CompositeId)
            .MustAsync(helper.IdentifyExistingComposite)
            .OverridePropertyName(ValidationMessages.CompositeIdPropName)
            .WithMessage(ValidationMessages.CompositeIdNotValidMessage);

        RuleFor(x => x.StoreItemId)
            .MustAsync(helper.IdentifyExistingStoreItem)
            .OverridePropertyName(ValidationMessages.StoreItemIdPropName)
            .WithMessage(ValidationMessages.StoreItemIdNotValidMessage);

        RuleFor(x => x.Amount)
            .InclusiveBetween(
                -ValidationConstants.MaxCompositionAmount,
                ValidationConstants.MaxCompositionAmount
            )
            .OverridePropertyName(ValidationMessages.AmountPropName)
            .WithMessage(ValidationMessages.AmountOutOfRangeMessage);


    }
}
