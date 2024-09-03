namespace KisV4.Common.Models;

public record ModifierAmountListModel(
    ModifierListModel Modifier,
    int SaleTransactionItemId,
    int Amount
);

public record ModifierAmountCreateModel(
    int ModifierId,
    int SaleTransactionItemId,
    int Amount
);