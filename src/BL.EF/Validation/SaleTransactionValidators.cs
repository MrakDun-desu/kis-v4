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
            .OverridePropertyName(ValidationMessages.DateRangePropName)
            .WithMessage(ValidationMessages.BadDateRangeMessage);
    }
}

public class SaleTransactionCreateValidator : AbstractValidator<SaleTransactionCreateRequest> {
    public SaleTransactionCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.Note)
            .MaximumLength(ValidationConstants.MaxNoteLength)
            .OverridePropertyName(ValidationMessages.NotePropName)
            .WithMessage(ValidationMessages.NoteTooLongMessage);
        RuleFor(x => x.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);
        RuleFor(x => x.CashBoxId)
            .MustAsync(helper.IdentifyExistingCashBox)
            .OverridePropertyName(ValidationMessages.CashBoxIdPropName)
            .WithMessage(ValidationMessages.CashBoxIdNotValidMessage);
        RuleFor(x => x.PaidAmount)
            .GreaterThanOrEqualTo(0)
            .OverridePropertyName(ValidationMessages.PaidAmountPropName)
            .WithMessage(ValidationMessages.PaidAmountLessThan0Message);
        RuleForEach(x => x.SaleTransactionItems)
            .SetValidator(new SaleTransactionItemCreateValidator());
        RuleFor(x => x)
            .MustAsync((x, token) => helper.PaidAmountIsEnough(x.SaleTransactionItems, x.PaidAmount, token))
            .OverridePropertyName(ValidationMessages.PaidAmountPropName)
            .WithMessage(ValidationMessages.PaidAmountTooLowMessage);
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.AllContainValidComposites)
            .OverridePropertyName(ValidationMessages.SaleTransactionItemsPropName)
            .WithMessage(ValidationMessages.SaleTransactionItemsNotValidCompositesMessage);
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.AllModifiersAreCorrect)
            .OverridePropertyName(ValidationMessages.SaleTransactionItemsPropName)
            .WithMessage(ValidationMessages.SaleTransactionItemModifiersNotValidMessage);
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.ItemAmountsArentNegative)
            .OverridePropertyName(ValidationMessages.SaleTransactionItemsPropName)
            .WithMessage(ValidationMessages.StoreTransactionItemAmountsNegativeMessage);
    }
}

public class SaleTransactionCheckPriceValidator : AbstractValidator<SaleTransactionCheckPriceRequest> {
    public SaleTransactionCheckPriceValidator(ValidationHelper helper) {
        RuleForEach(x => x.SaleTransactionItems)
            .SetValidator(new SaleTransactionItemCreateValidator());
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.AllModifiersAreCorrect)
            .OverridePropertyName(ValidationMessages.SaleTransactionItemsPropName)
            .WithMessage(ValidationMessages.SaleTransactionItemModifiersNotValidMessage);
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.AllContainValidComposites)
            .OverridePropertyName(ValidationMessages.SaleTransactionItemsPropName)
            .WithMessage(ValidationMessages.SaleTransactionItemsNotValidCompositesMessage);
    }
}

public class SaleTransactionOpenValidator : AbstractValidator<SaleTransactionOpenRequest> {
    public SaleTransactionOpenValidator(ValidationHelper helper) {
        RuleFor(x => x.Note)
            .MaximumLength(ValidationConstants.MaxNoteLength)
            .OverridePropertyName(ValidationMessages.NotePropName)
            .WithMessage(ValidationMessages.NoteTooLongMessage);
        RuleFor(x => x.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);
        RuleForEach(x => x.SaleTransactionItems)
            .SetValidator(new SaleTransactionItemCreateValidator());
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.AllContainValidComposites)
            .OverridePropertyName(ValidationMessages.SaleTransactionItemsPropName)
            .WithMessage(ValidationMessages.SaleTransactionItemsNotValidCompositesMessage);
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.ItemAmountsArentNegative)
            .OverridePropertyName(ValidationMessages.SaleTransactionItemsPropName)
            .WithMessage(ValidationMessages.StoreTransactionItemAmountsNegativeMessage);
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.AllModifiersAreCorrect)
            .OverridePropertyName(ValidationMessages.SaleTransactionItemsPropName)
            .WithMessage(ValidationMessages.SaleTransactionItemModifiersNotValidMessage);
    }
}

public class SaleTransactionUpdateValidator : AbstractValidator<SaleTransactionUpdateRequest> {
    public SaleTransactionUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.Model.Note)
            .MaximumLength(ValidationConstants.MaxNoteLength)
            .OverridePropertyName(ValidationMessages.NotePropName)
            .WithMessage(ValidationMessages.NoteTooLongMessage);
        RuleFor(x => x.Model.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);
        RuleForEach(x => x.Model.SaleTransactionItems)
            .SetValidator(new SaleTransactionItemCreateValidator());
        RuleFor(x => x.Model.SaleTransactionItems)
            .MustAsync(helper.AllContainValidComposites)
            .OverridePropertyName(ValidationMessages.SaleTransactionItemsPropName)
            .WithMessage(ValidationMessages.SaleTransactionItemsNotValidCompositesMessage);
        RuleFor(x => x.Model.SaleTransactionItems)
            .MustAsync(helper.ItemAmountsArentNegative)
            .OverridePropertyName(ValidationMessages.SaleTransactionItemsPropName)
            .WithMessage(ValidationMessages.StoreTransactionItemAmountsNegativeMessage);
        RuleFor(x => x.Model.SaleTransactionItems)
            .MustAsync(helper.AllModifiersAreCorrect)
            .OverridePropertyName(ValidationMessages.SaleTransactionItemsPropName)
            .WithMessage(ValidationMessages.SaleTransactionItemModifiersNotValidMessage);
    }
}

public class SaleTransactionCloseValidator : AbstractValidator<SaleTransactionCloseRequest> {
    public SaleTransactionCloseValidator(ValidationHelper helper) {
        RuleFor(x => x.Model.Note)
            .MaximumLength(ValidationConstants.MaxNoteLength)
            .OverridePropertyName(ValidationMessages.NotePropName)
            .WithMessage(ValidationMessages.NoteTooLongMessage);
        RuleFor(x => x.Model.CashBoxId)
            .MustAsync(helper.IdentifyExistingCashBox)
            .OverridePropertyName(ValidationMessages.CashBoxIdPropName)
            .WithMessage(ValidationMessages.CashBoxIdNotValidMessage);
        RuleFor(x => x.Model.PaidAmount)
            .GreaterThanOrEqualTo(0)
            .OverridePropertyName(ValidationMessages.PaidAmountPropName)
            .WithMessage(ValidationMessages.PaidAmountLessThan0Message);
        RuleFor(x => x)
            .MustAsync((x, token) => helper.PaidAmountIsEnough(x.Id, x.Model.PaidAmount, token))
            .OverridePropertyName(ValidationMessages.PaidAmountPropName)
            .WithMessage(ValidationMessages.PaidAmountTooLowMessage);
    }
}
