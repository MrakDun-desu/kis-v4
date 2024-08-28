namespace KisV4.Common.Models;

public record Page<T>(
    ICollection<T> Data,
    PageMeta Meta
);

public record PageMeta(
    int Page,
    int PageSize,
    int From,
    int To,
    int Total,
    int PageCount
);