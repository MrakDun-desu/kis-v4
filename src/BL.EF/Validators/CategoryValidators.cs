using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateRequest> {
    public CategoryCreateValidator() {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateRequest> {
    public CategoryUpdateValidator() {
        RuleFor(x => x.Name).NotEmpty();
    }
}
