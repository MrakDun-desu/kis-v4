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
    public SaleTransactionCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.Note)
            .MaximumLength(ValidationConstants.MaxNoteLength);
        RuleFor(x => x.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .WithMessage("Store ID must identify an existing store");
        RuleFor(x => x.CashBoxId)
            .MustAsync(helper.IdentifyExistingCashBox)
            .WithMessage("Cash-box ID must identify an existing cash-box");
        RuleFor(x => x.PaidAmount)
            .GreaterThanOrEqualTo(0);
        RuleForEach(x => x.SaleTransactionItems)
            .SetValidator(new SaleTransactionItemCreateValidator());
        RuleFor(x => x)
            .MustAsync((x, token) => helper.PaidAmountIsEnough(x.SaleTransactionItems, x.PaidAmount, token))
            .WithMessage("The paid amount is not enough to pay for this transaction");
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.AllContainValidComposites)
            .WithMessage("""
                All sale item IDs and modifier IDs must identify correct entities
                """);
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.AllModifiersAreCorrect)
            .WithMessage("""
                All modifiers must have their modified sale item specified as a valid target
                """);
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.ItemAmountsArentNegative)
            .WithMessage("Attempting to sell a negative amount of a store item");
    }
}

public class SaleTransactionCheckPriceValidator : AbstractValidator<SaleTransactionCheckPriceRequest> {
    public SaleTransactionCheckPriceValidator(ValidationHelper helper) {
        RuleForEach(x => x.SaleTransactionItems)
            .SetValidator(new SaleTransactionItemCreateValidator());
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.AllModifiersAreCorrect)
            .WithMessage("""
                All modifiers must have their modified sale item specified as a valid target
                """);
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.AllContainValidComposites)
            .WithMessage("""
                All sale item IDs and modifier IDs must identify correct entities
                """);
    }
}

public class SaleTransactionOpenValidator : AbstractValidator<SaleTransactionOpenRequest> {
    public SaleTransactionOpenValidator(ValidationHelper helper) {
        RuleFor(x => x.Note)
            .MaximumLength(ValidationConstants.MaxNoteLength);
        RuleFor(x => x.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .WithMessage("Store ID must identify an existing store");
        RuleForEach(x => x.SaleTransactionItems)
            .SetValidator(new SaleTransactionItemCreateValidator());
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.AllContainValidComposites)
            .WithMessage("""
                All sale item IDs and modifier IDs must identify correct entities
                """);
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.ItemAmountsArentNegative)
            .WithMessage("Attempting to sell a negative amount of a store item");
        RuleFor(x => x.SaleTransactionItems)
            .MustAsync(helper.AllModifiersAreCorrect)
            .WithMessage("""
                All modifiers must have their modified sale item specified as a valid target
                """);
    }
}

public class SaleTransactionUpdateValidator : AbstractValidator<SaleTransactionUpdateRequest> {
    public SaleTransactionUpdateValidator(ValidationHelper helper) {
        RuleFor(x => x.Model.Note)
                .MaximumLength(ValidationConstants.MaxNoteLength);
        RuleFor(x => x.Model.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .WithMessage("Store ID must identify an existing store");
        RuleForEach(x => x.Model.SaleTransactionItems)
            .SetValidator(new SaleTransactionItemCreateValidator());
        RuleFor(x => x.Model.SaleTransactionItems)
            .MustAsync(helper.AllContainValidComposites)
            .WithMessage("""
                        All sale item IDs and modifier IDs must identify correct entities
                        """);
        RuleFor(x => x.Model.SaleTransactionItems)
            .MustAsync(helper.ItemAmountsArentNegative)
            .WithMessage("Attempting to sell a negative amount of a store item");
        RuleFor(x => x.Model.SaleTransactionItems)
            .MustAsync(helper.AllModifiersAreCorrect)
            .WithMessage("""
                All modifiers must have their modified sale item specified as a valid target
                """);
    }
}

public class SaleTransactionCloseValidator : AbstractValidator<SaleTransactionCloseRequest> {
    public SaleTransactionCloseValidator(ValidationHelper helper) {
        RuleFor(x => x.Model.Note)
                .MaximumLength(ValidationConstants.MaxNoteLength);
        RuleFor(x => x.Model.CashBoxId)
            .MustAsync(helper.IdentifyExistingCashBox)
            .WithMessage("Cash-box ID must identify an existing cash-box");
        RuleFor(x => x.Model.PaidAmount)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x)
            .MustAsync((x, token) => helper.PaidAmountIsEnough(x.Id, x.Model.PaidAmount, token))
            .WithMessage("The paid amount is not enough to pay for this transaction");
    }
}
