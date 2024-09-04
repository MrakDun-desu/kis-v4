using KisV4.Common.Models;
using OneOf;

namespace KisV4.BL.EF;

public static class IQueryableExtensions
{
    public static OneOf<Page<TTarget>, Dictionary<string, string[]>> Page<TSource, TTarget>(
        this IQueryable<TSource> source,
        int page,
        int pageSize,
        Func<List<TSource>, List<TTarget>> mapping)
    {
        if (page < 1)
            return new Dictionary<string, string[]>
            {
                { nameof(page), [$"Page is required to be higher than 0. Received value: {page}"] }
            };

        var totalCount = source.Count();
        var pageCount = totalCount / pageSize + 1;
        switch (totalCount)
        {
            case > 0 when page > pageCount:
                return new Dictionary<string, string[]>
                {
                    {
                        nameof(page),
                        [$"Page is required to be lower or equal to page count ({pageCount}). Received value: {page}"]
                    }
                };
            case 0:
                return new Page<TTarget>([], new PageMeta(0, 0, 0, 0, 0, 0));
        }

        var skipped = (page - 1) * pageSize;
        var data = source.Skip(skipped).Take(pageSize).ToList();
        var from = skipped + 1;
        var count = data.Count;

        return new Page<TTarget>(
            mapping.Invoke(data),
            new PageMeta(page, pageSize, from, from + count - 1, totalCount, pageCount)
        );
    }
}