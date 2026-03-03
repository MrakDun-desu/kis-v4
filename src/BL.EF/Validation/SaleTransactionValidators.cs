using FluentValidation;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class SaleTransactionReadAllValidator : AbstractValidator<SaleTransactionReadAllRequest> {
    public SaleTransactionReadAllValidator() {
        Include(new PagedRequestValidator());
        RuleFor(x => x)
            .Must(x =>
                // if either of them is null, no need to check
                x.From is null || x.To is null || x.From < x.To
            )
            .WithMessage("The datetime From must be earlier than the datetime To if both of them are set");
    }
}

public class SaleTransactionCreateValidator : AbstractValidator<SaleTransactionCreateRequest> {
    public SaleTransactionCreateValidator() {
        // TODO
    }
}

public class SaleTransactionCheckPriceValidator : AbstractValidator<SaleTransactionCheckPriceRequest> {
    public SaleTransactionCheckPriceValidator() {
        // TODO
    }
}

public class SaleTransactionOpenValidator : AbstractValidator<SaleTransactionOpenRequest> {
    public SaleTransactionOpenValidator() {
        // TODO
    }
}

public class SaleTransactionUpdateValidator : AbstractValidator<SaleTransactionUpdateRequest> {
    public SaleTransactionUpdateValidator() {
        // TODO
    }
}

public class SaleTransactionCloseValidator : AbstractValidator<SaleTransactionCloseRequest> {
    public SaleTransactionCloseValidator() {
        // TODO
    }
}
