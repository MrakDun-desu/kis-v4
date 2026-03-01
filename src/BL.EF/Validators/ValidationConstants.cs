namespace KisV4.BL.EF.Validators;

public static class ValidationConstants {
    // string lengths
    public const int MaxNameLength = 30;
    public const int MaxNoteLength = 200;
    public const int MaxUnitNameLength = 10;
    public const int MaxDescriptionLength = 200;

    // pagination
    public const int MaxPageSize = 100;

    // costs
    public const decimal MaxAllowedCost = 100_000;
    public const decimal MaxMarginPercent = 300;

    // amounts
    public const decimal MaxTransactionAmount = 100_000;
    public const decimal MaxCompositionAmount = 100_000;

    // layouts
    public const int LayoutWidth = 4;
    public const int LayoutHeight = 4;
}
