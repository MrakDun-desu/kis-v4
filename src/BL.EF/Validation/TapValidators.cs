using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class TapCreateValidator : AbstractValidator<TapCreateRequest> {
    public TapCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
        RuleFor(x => x.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);
    }
}

public class TapUpdateValidator : AbstractValidator<TapUpdateRequest> {
    public TapUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.Model.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);
        RuleFor(x => x.Model.ContainerId)
            .MustAsync(helper.BeNullOrIdentifyExistingContainer)
            .OverridePropertyName(ValidationMessages.ContainerIdPropName)
            .WithMessage(ValidationMessages.ContainerIdNotValidMessage)
            .MustAsync(helper.IdentifyAnAvailableContainer)
            .OverridePropertyName(ValidationMessages.ContainerIdPropName)
            .WithMessage(ValidationMessages.ContainerNotAvailableMessage);
        RuleFor(x => x)
            .MustAsync(helper.BeContainerWithNewStoreItem)
            .OverridePropertyName(ValidationMessages.ContainerIdPropName)
            .WithMessage(ValidationMessages.SameContainerAlreadyTappedMessage);
        RuleFor(x => x)
            .MustAsync(helper.NotHaveContainerIfAddingNew)
            .WithMessage(ValidationMessages.CantAddContainerToBusyPipe);
    }
}
