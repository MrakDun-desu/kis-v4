using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class ContainerReadAllValidator : AbstractValidator<ContainerReadAllRequest> {
    public ContainerReadAllValidator(ValidationHelper helper) {
        Include(new PagedRequestValidator());

        RuleFor(x => x.StoreId)
            .MustAsync(helper.BeNullOrIdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);
        RuleFor(x => x.TemplateId)
            .MustAsync(helper.BeNullOrIdentifyExistingTemplate)
            .OverridePropertyName(ValidationMessages.ContainerTemplateIdPropName)
            .WithMessage(ValidationMessages.ContainerTemplateIdNotValidMessage);
        RuleFor(x => x.PipeId)
            .MustAsync(helper.BeNullOrIdentifyExistingPipe)
            .OverridePropertyName(ValidationMessages.PipeIdPropName)
            .WithMessage(ValidationMessages.PipeIdNotValidMessage);
    }

}

public class ContainerCreateValidator : AbstractValidator<ContainerCreateRequest> {
    public ContainerCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);

        RuleFor(x => x.TemplateId)
            .MustAsync(helper.IdentifyExistingTemplate)
            .OverridePropertyName(ValidationMessages.ContainerTemplateIdPropName)
            .WithMessage(ValidationMessages.ContainerTemplateIdNotValidMessage);
    }

}

public class ContainerUpdateValidator : AbstractValidator<ContainerUpdateRequest> {
    public ContainerUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.Model.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);
        RuleFor(x => x.Model.PipeId)
            .MustAsync(helper.BeNullOrIdentifyExistingPipe)
            .OverridePropertyName(ValidationMessages.PipeIdPropName)
            .WithMessage(ValidationMessages.PipeIdNotValidMessage);
    }

}
