using KisV4.BL.EF.Helpers;
using KisV4.Common;
using KisV4.Common.Models;
using OneOf;

namespace KisV4.BL.EF;

public static class IQueryableExtensions {
    public static OneOf<Page<TTarget>, Dictionary<string, string[]>> Page<TSource, TTarget>(
        this IQueryable<TSource> source,
        int page,
        int pageSize,
        Func<List<TSource>, List<TTarget>> mapping) {
        var errors = new Dictionary<string, string[]>();
        if (page < 1) {
            errors.AddItemOrCreate(
                nameof(page), $"Page is required to be higher than 0. Received value: {page}"
            );
        }

        pageSize = Math.Min(pageSize, Constants.MaxPageSize);

        if (errors.Count > 0) {
            return errors;
        }

        var totalCount = source.Count();
        var pageCount = (totalCount / pageSize) + 1;
        if (totalCount == 0) {
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
