using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class ContainerChangeReadAllValidator : AbstractValidator<ContainerChangeReadAllRequest> {
    public ContainerChangeReadAllValidator(ValidationHelper helper) {
        RuleFor(x => x.ContainerId)
            .MustAsync(helper.IdentifyExistingContainer)
            .OverridePropertyName(ValidationMessages.ContainerIdPropName)
            .WithMessage(ValidationMessages.ContainerIdNotValidMessage);
    }
}

public class ContainerChangeCreateValidator : AbstractValidator<ContainerChangeCreateRequest> {
    public ContainerChangeCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.NewAmount)
            .GreaterThanOrEqualTo(0)
            .OverridePropertyName(ValidationMessages.AmountPropName)
            .WithMessage(ValidationMessages.AmountTooLowMessage);
        RuleFor(x => x)
            .MustAsync(helper.HaveAmountLowerOrEqualToCurrent)
            .OverridePropertyName(ValidationMessages.AmountPropName)
            .WithMessage(ValidationMessages.ContainerAmountTooGreatMessage);
        RuleFor(x => x)
            .MustAsync(helper.HaveCorrectStateTransition)
            .OverridePropertyName(ValidationMessages.ContainerStatePropName)
            .WithMessage(ValidationMessages.ContainerInvalidNewStateMessage);
        RuleFor(x => x.ContainerId)
            .MustAsync(helper.IdentifyExistingContainer)
            .OverridePropertyName(ValidationMessages.ContainerIdPropName)
            .WithMessage(ValidationMessages.ContainerIdNotValidMessage);
    }

}
