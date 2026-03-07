using FluentValidation;
using KisV4.Common.Enums;
using KisV4.Common.Models;

namespace KisV4.BL.EF.Validation;

public class StoreTransactionReadAllValidator : AbstractValidator<StoreTransactionReadAllRequest> {
    public StoreTransactionReadAllValidator() {
        Include(new PagedRequestValidator());
        RuleFor(x => x)
            .Must(x => {
                // if either of them is null, no need to check
                return x.From is null || x.To is null || x.From < x.To;
            })
            .OverridePropertyName(ValidationMessages.DateRangePropName)
            .WithMessage(ValidationMessages.BadDateRangeMessage);
    }
}

public class StoreTransactionCreateValidator : AbstractValidator<StoreTransactionCreateRequest> {
    public StoreTransactionCreateValidator(ValidationHelper helper) {
        RuleFor(x => x.Note)
            .MaximumLength(ValidationConstants.MaxNoteLength)
            .OverridePropertyName(ValidationMessages.NotePropName)
            .WithMessage(ValidationMessages.NoteTooLongMessage);
        RuleFor(x => x.Reason)
            .NotEqual(TransactionReason.Sale)
            .OverridePropertyName(ValidationMessages.TransactionReasonPropName)
            .WithMessage(ValidationMessages.CantCreateStoreTransactionAsSaleMessage);
        RuleFor(x => x.StoreId)
            .MustAsync(helper.IdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);
        RuleFor(x => x.SourceStoreId)
            .MustAsync(helper.BeNullOrIdentifyExistingStore)
            .OverridePropertyName(ValidationMessages.StoreIdPropName)
            .WithMessage(ValidationMessages.StoreIdNotValidMessage);
        RuleFor(x => x)
            .Must(x => x.StoreId != x.SourceStoreId)
            .WithMessage(ValidationMessages.SourceStoreSameAsTargetStoreMessage);
        RuleFor(x => x)
            .Must(x => x.Reason == TransactionReason.ChangingStores && x.SourceStoreId is not null ||
                        x.Reason != TransactionReason.ChangingStores && x.SourceStoreId is null)
            .WithMessage(ValidationMessages.StoreTransactionReasonAndSourceStoreInvalidMessage);

        RuleForEach(x => x.StoreTransactionItems)
            .SetValidator(new StoreTransactionItemCreateValidator());
        RuleFor(x => x.StoreTransactionItems)
            .Must(x => x.Select(sti => sti.StoreItemId).Distinct().Count() == x.Count())
            .OverridePropertyName(ValidationMessages.StoreTransactionItemsPropName)
            .WithMessage(ValidationMessages.StoreTransactionItemsNotUniqueMessage);
        RuleFor(x => x.StoreTransactionItems)
            .MustAsync(helper.AllHaveNonContainerStoreItems)
            .OverridePropertyName(ValidationMessages.StoreTransactionItemsPropName)
            .WithMessage(ValidationMessages.StoreTransactionItemsContainContainerItemsMessage);
        RuleFor(x => x.StoreTransactionItems)
            .MustAsync(helper.AllHaveExistingStoreItems)
            .OverridePropertyName(ValidationMessages.StoreTransactionItemsPropName)
            .WithMessage(ValidationMessages.StoreItemIdsNotValidMessage);
    }
}
