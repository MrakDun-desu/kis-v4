namespace KisV4.Common.Authorization;

public static class AuthorizationConstants {
    public static readonly TimeSpan TransactionCancelTimeout = TimeSpan.FromMinutes(30);

    public const string KisFoodHttpClientName = "kis_food";
}
