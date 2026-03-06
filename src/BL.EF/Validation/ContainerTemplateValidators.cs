using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class ContainerTemplateReadAllValidator : AbstractValidator<ContainerTemplateReadAllRequest> {
    public ContainerTemplateReadAllValidator(ValidationHelper helper) {
        Include(new PagedRequestValidator());

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

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .LessThan(ValidationConstants.MaxTransactionAmount);
    }
}

public class ContainerTemplateUpdateValidator : AbstractValidator<ContainerTemplateUpdateRequest> {
    public ContainerTemplateUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.Model.StoreItemId)
            .MustAsync(helper.IdentifyExistingContainerItem)
            .WithMessage("Specified store item must be an existing container item");

        RuleFor(x => x.Model.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);

        RuleFor(x => x)
            .MustAsync(helper.NotHaveExistingContainers)
            .WithMessage("Container templates that have existing containers can't be updated");
    }
}

public class ContainerTemplateDeleteValidator : AbstractValidator<ContainerTemplateDeleteRequest> {
    public ContainerTemplateDeleteValidator(ValidationHelper helper) {
        RuleFor(x => x)
            .MustAsync(helper.NotHaveAnyAssociatedContainers)
            .WithMessage("Can't delete container templates with active containers");
    }
}
