using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class PipeCreateValidator : AbstractValidator<PipeCreateRequest> {
    public PipeCreateValidator() {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
    }
}

public class PipeUpdateValdiator : AbstractValidator<PipeUpdateRequest> {
    public PipeUpdateValdiator() {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
    }
}
