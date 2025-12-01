namespace KisV4.Common.ModelWrappers;

public abstract record PagedRequest {
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 30;
}

public record PageMeta {
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int Total { get; init; }
}

public abstract record PagedResponse<T> {
    public required T[] Data { get; init; }
    public required PageMeta Meta { get; init; }
}
