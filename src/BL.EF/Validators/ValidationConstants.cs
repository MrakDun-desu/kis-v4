namespace KisV4.BL.EF.Validators;

public static class ValidationConstants {

    public const int MaxNameLength = 30;
    public const int MaxUnitNameLength = 10;

    public const int MaxDescriptionLength = 200;

    public const int MaxPageSize = 100;

    public const decimal MaxAllowedCost = 100_000;

    public const decimal MaxTransactionAmount = 100_000;
    public const decimal MinTransactionAmount = -100_000;

    public const decimal MaxCompositionAmount = 100_000;
    public const decimal MinCompositionAmount = -100_000;
}
