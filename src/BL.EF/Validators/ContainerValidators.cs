using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class ContainerReadAllValidator : AbstractValidator<ContainerReadAllRequest> {
    public ContainerReadAllValidator(ValidationHelper helper) {
        Include(new PagedRequestValidator());

        RuleFor(x => x.StoreId)
            .MustAsync(helper.BeNullOrIdentifyExistingStore)
            .WithMessage("StoreId must either be null or identify an existing store");
        RuleFor(x => x.TemplateId)
            .MustAsync(helper.BeNullOrIdentifyExistingTemplate)
            .WithMessage("TemplateId must either be null or identify an existing template");
        RuleFor(x => x.PipeId)
            .MustAsync(helper.BeNullOrIdentifyExistingPipe)
            .WithMessage("PipeId must either be null or identify an existing pipe");
    }

}

public class ContainerCreateValidator : AbstractValidator<ContainerCreateRequest> {
    public ContainerCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .WithMessage("Specified store must exist");

        RuleFor(x => x.TemplateId)
            .MustAsync(helper.IdentifyExistingTemplate)
            .WithMessage("Specified container template must exist");
    }

}

public class ContainerUpdateValidator : AbstractValidator<ContainerUpdateRequest> {
    public ContainerUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .WithMessage("Specified store must exist");
        RuleFor(x => x.PipeId)
            .MustAsync(helper.BeNullOrIdentifyExistingPipe)
            .WithMessage("PipeId must either be null or identify an existing pipe");
    }

}
