using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

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

public class ContainerTemplateUpdateValidator : AbstractValidator<ContainerTemplateUpdateCommand> {
    public ContainerTemplateUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.Request.StoreItemId)
            .MustAsync(helper.IdentifyExistingContainerItem)
            .WithMessage("Specified store item must be an existing container item");

        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);

        RuleFor(x => x)
            .MustAsync(helper.NotHaveExistingContainers)
            .WithMessage("Container templates that have existing containers can't be updated");
    }
}
