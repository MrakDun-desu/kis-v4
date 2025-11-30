using KisV4.Common.ModelWrappers;

namespace KisV4.BL.EF;

public static class IQueryableExtensions {

    public static TOutPage KeysetPaginate<TOutPage, TOut, TSource, TKey>(
            this IQueryable<TSource> source,
            KeysetPagedRequest<TKey> req,
            Func<TSource, TOut> mapping,
            Func<IEnumerable<TOut>, KeysetPageMeta<TKey>, TOutPage> factory,
            Func<TSource, TKey> order,
            bool orderDesc = false
            )
    where TOutPage : KeysetPagedResponse<TKey, TOut>
    where TKey : struct, IComparable<TKey> {
        var total = source.Count();

        var ordered = orderDesc
            ? source.OrderByDescending(order)
            : source.OrderBy(order);

        var offsetCollection = req.PageStart switch {
            null => source,
            { } pageStart => orderDesc
                ? source.Where(s => order(s).CompareTo(pageStart) <= 0)
                : source.Where(s => order(s).CompareTo(pageStart) >= 0)
        };

        var queried = offsetCollection
            .Take(req.PageSize + 1)
            .ToArray();

        TKey? nextPageStart = (queried.Length > req.PageSize)
            ? null
            : order(queried[req.PageSize]);

        TKey? realPageStart = req.PageStart ?? queried.FirstOrDefault() switch {
            null => null,
            var val => order(val)
        };

        return factory(
            queried[..req.PageSize].Select(mapping).ToArray(),
            new KeysetPageMeta<TKey> {
                Total = total,
                PageStart = realPageStart,
                NextPageStart = nextPageStart,
                PageSize = req.PageSize,
            }
        );
    }

    public static TOutPage Paginate<TOutPage, TOut, TSource, TOrder>(
            this IQueryable<TSource> source,
            PagedRequest req,
            Func<TSource, TOut> mapping,
            Func<IEnumerable<TOut>, PageMeta, TOutPage> factory,
            Func<TSource, TOrder> order,
            bool orderDesc = false
            )
    where TOutPage : PagedResponse<TOut> {
        var total = source.Count();

        var ordered = orderDesc switch {
            true => source.OrderByDescending(order),
            false => source.OrderBy(order)
        };

        return factory(
            source
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .Select(mapping)
                .ToArray(),
            new PageMeta {
                Page = req.Page,
                PageSize = req.PageSize,
                Total = total
            }
        );
    }
}
