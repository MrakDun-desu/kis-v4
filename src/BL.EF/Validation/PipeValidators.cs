using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class PipeCreateValidator : AbstractValidator<PipeCreateRequest> {
    public PipeCreateValidator() {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
    }
}

public class PipeUpdateValidator : AbstractValidator<PipeUpdateRequest> {
    public PipeUpdateValidator() {
        RuleFor(x => x.Model.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxNameLength);
    }
}
