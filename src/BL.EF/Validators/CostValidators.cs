using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class CostsCreateValidator : AbstractValidator<CostCreateRequest> {
    public CostsCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.StoreItemId)
            .MustAsync(helper.IdentifyExistingStoreItem)
            .WithMessage("Specified store item must exist");
    }
}
