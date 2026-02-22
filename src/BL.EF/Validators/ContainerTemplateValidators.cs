using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class ContainerTemplateReadAllValidator : AbstractValidator<ContainerTemplateReadAllRequest> {
    public ContainerTemplateReadAllValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreItemId)
            .MustAsync(helper.BeNullOrIdentifyExistingContainerItem)
            .WithMessage("The queried store item must be null or container item");
    }
}

public class ContainerTemplateCreateValidator : AbstractValidator<ContainerTemplateCreateRequest> {
    public ContainerTemplateCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreItemId)
            .MustAsync(helper.IdentifyExistingContainerItem)
            .WithMessage("Specified store item must be an existing container item");
    }
}

public class ContainerTemplateUpdateValidator : AbstractValidator<ContainerTemplateUpdateRequest> {
    public ContainerTemplateUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreItemId)
            .MustAsync(helper.IdentifyExistingContainerItem)
            .WithMessage("Specified store item must be an existing container item");
    }
}
