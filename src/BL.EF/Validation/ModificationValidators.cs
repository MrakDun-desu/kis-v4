using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class ModificationCreateValidator : AbstractValidator<ModificationCreateRequest> {
    public ModificationCreateValidator() {
        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}
