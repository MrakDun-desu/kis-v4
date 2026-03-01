using FluentValidation;
using KisV4.Common.Enums;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validators;

public class StoreTransactionReadAllValidator : AbstractValidator<StoreTransactionReadAllRequest> {
    public StoreTransactionReadAllValidator() {
        Include(new PagedRequestValidator());
        RuleFor(x => x)
            .Must(x => {
                // if either of them is null, no need to check
                return x.From is null || x.To is null || x.From < x.To;
            })
            .WithMessage("The datetime From must be earlier than the datetime To if both of them are set");
    }
}

public class StoreTransactionCreateValidator : AbstractValidator<StoreTransactionCreateRequest> {
    public StoreTransactionCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.Note)
            .MaximumLength(ValidationConstants.MaxNoteLength);
        RuleFor(x => x.Reason)
            .NotEqual(TransactionReason.Sale)
            .WithMessage("Can't create a store transaction as a sale individually. Use sale transaction endpoints instead");
        RuleFor(x => x.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .WithMessage("Store ID must identify existing store");
        RuleFor(x => x.SourceStoreId)
            .MustAsync(helper.BeNullOrIdentifyExistingStore)
            .WithMessage("Source store ID must be null or identify existing store");
        RuleFor(x => x)
            .Must(x => x.StoreId != x.SourceStoreId)
            .WithMessage("Source store ID needs to be different from target store ID");
        RuleFor(x => x)
            .Must(x => x.Reason == TransactionReason.ChangingStores && x.SourceStoreId is not null ||
                        x.Reason != TransactionReason.ChangingStores && x.SourceStoreId is null)
            .WithMessage("Source store ID should be set exactly when transaction reason is changing stores");

        RuleForEach(x => x.StoreTransactionItems)
            .SetValidator(new StoreTransactionItemCreateValidator());
        RuleFor(x => x.StoreTransactionItems)
            .Must(x => x.Select(sti => sti.StoreItemId).Distinct().Count() == x.Count())
            .WithMessage("Store item IDs are not unique");
        RuleFor(x => x.StoreTransactionItems)
            .MustAsync(helper.AllHaveNonContainerStoreItems)
            .WithMessage("Can't create a store transaction for container items. Use container endpoints instead");
        RuleFor(x => x.StoreTransactionItems)
            .MustAsync(helper.AllHaveExistingStoreItems)
            .WithMessage("All the store item IDs must identify existing store items");
    }
}
