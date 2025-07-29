namespace KisV4.Common;

public static class Constants {
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 50;
    public static readonly TimeSpan SelfCancellableTime = TimeSpan.FromMinutes(15);
    public static readonly TimeSpan FinishedTransactionChangeTime = TimeSpan.FromMinutes(15);
}
