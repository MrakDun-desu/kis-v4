using System.ComponentModel;

namespace KisV4.Common.ModelWrappers;

public abstract record KeysetPagedRequest<TKey> where TKey : struct {
    public TKey? PageStart { get; init; }
    public int? PageSize { get; init; }
}

public record KeysetPageMeta<TKey> where TKey : struct {
    public required TKey? PageStart { get; init; }
    public required int PageSize { get; init; }
    public required TKey? NextPageStart { get; init; }
    public required int Total { get; init; }
}

public abstract record KeysetPagedResponse<TKey, TValue> where TKey : struct {
    public required TValue[] Data { get; init; }
    public required KeysetPageMeta<TKey> Meta { get; init; }
}
