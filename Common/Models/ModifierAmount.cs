namespace KisV4.Common.Models;

public record ModifierAmountReadAllModel(
    ModifierReadAllModel Modifier,
    int SaleTransactionItemId,
    int Amount
);