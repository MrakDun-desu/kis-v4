using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class ContainerChangeReadAllValidator : AbstractValidator<ContainerChangeReadAllRequest> {
    public ContainerChangeReadAllValidator(ValidationHelper helper) {
        RuleFor(x => x.ContainerId)
            .MustAsync(helper.IdentifyExistingContainer)
            .WithMessage("Specified container must exist");
    }
}

public class ContainerChangeCreateValidator : AbstractValidator<ContainerChangeCreateRequest> {
    public ContainerChangeCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.NewAmount)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.ContainerId)
            .MustAsync(helper.IdentifyExistingContainer)
            .WithMessage("Specified container must exist");
        RuleFor(x => x)
            .MustAsync(helper.HaveAmountLowerOrEqualToCurrent)
            .WithMessage("New container amount must be lower or equal to the current one");
    }

}
