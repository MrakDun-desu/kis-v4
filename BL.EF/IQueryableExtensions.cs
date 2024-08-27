using KisV4.Common.Models;

namespace KisV4.BL.EF;

public static class IQueryableExtensions
{
    public static Page<TTarget> Page<TSource, TTarget>(
        this IQueryable<TSource> source,
        int page,
        int pageSize,
        Func<List<TSource>, List<TTarget>> mapping)
    {
        var skipped = (page - 1) * pageSize;
        var data = source.Skip(skipped).Take(pageSize).ToList();
        var from = skipped + 1;
        var realSize = data.Count;
        var total = source.Count();
        return new Page<TTarget>(
            mapping.Invoke(data),
            new PageMeta(page, pageSize, from, from + realSize - 1, total, total / pageSize + 1)
        );
    }
}