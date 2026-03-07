using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class ContainerTemplateReadAllValidator : AbstractValidator<ContainerTemplateReadAllRequest> {
    public ContainerTemplateReadAllValidator(ValidationHelper helper) {
        Include(new PagedRequestValidator());

        RuleFor(x => x.StoreItemId)
            .MustAsync(helper.BeNullOrIdentifyExistingContainerItem)
            .OverridePropertyName(ValidationMessages.StoreItemIdPropName)
            .WithMessage(ValidationMessages.ContainerItemIdNotValidMessage);
    }
}

public class ContainerTemplateCreateValidator : AbstractValidator<ContainerTemplateCreateRequest> {
    public ContainerTemplateCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreItemId)
            .MustAsync(helper.IdentifyExistingContainerItem)
            .OverridePropertyName(ValidationMessages.StoreItemIdPropName)
            .WithMessage(ValidationMessages.ContainerItemIdNotValidMessage);

        RuleFor(x => x.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);

        RuleFor(x => x.Amount)
            .InclusiveBetween(0, ValidationConstants.MaxTransactionAmount)
            .OverridePropertyName(ValidationMessages.AmountPropName)
            .WithMessage(ValidationMessages.AmountOutOfRangeMessage);
    }
}

public class ContainerTemplateUpdateValidator : AbstractValidator<ContainerTemplateUpdateRequest> {
    public ContainerTemplateUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.Model.StoreItemId)
            .MustAsync(helper.IdentifyExistingContainerItem)
            .OverridePropertyName(ValidationMessages.StoreItemIdPropName)
            .WithMessage(ValidationMessages.ContainerItemIdNotValidMessage);

        RuleFor(x => x.Model.Name)
            .MaximumLength(ValidationConstants.MaxNameLength)
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameTooLongMessage)
            .NotEmpty()
            .OverridePropertyName(ValidationMessages.NamePropName)
            .WithMessage(ValidationMessages.NameEmptyMessage);

        RuleFor(x => x)
            .MustAsync(helper.NotHaveExistingContainers)
            .WithMessage(ValidationMessages.ContainerTemplateUsedMessage);
    }
}

public class ContainerTemplateDeleteValidator : AbstractValidator<ContainerTemplateDeleteRequest> {
    public ContainerTemplateDeleteValidator(ValidationHelper helper) {
        RuleFor(x => x)
            .MustAsync(helper.NotHaveAnyAssociatedContainers)
            .WithMessage(ValidationMessages.ContainerTemplateUsedMessage);
    }
}
