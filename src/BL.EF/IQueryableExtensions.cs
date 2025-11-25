using KisV4.Common.ModelWrappers;

namespace KisV4.BL.EF;

public static class IQueryableExtensions {

    public static TOutPage KeysetPaginate<TOutPage, TOut, TSource, TKey>(
            this IQueryable<TSource> source,
            KeysetPagedRequest<TKey> req,
            Func<TSource, TOut> mapping,
            Func<TOut[], KeysetPageMeta<TKey>, TOutPage> factory,
            Func<TSource, TKey> order,
            bool orderDesc = false
            )
    where TOutPage : KeysetPagedResponse<TKey, TOut>
    where TKey : struct, IComparable<TKey> {
        var total = source.Count();

        var ordered = orderDesc
            ? source.OrderByDescending(order)
            : source.OrderBy(order);

        IEnumerable<TSource> offsetCollection = orderDesc
            ? source.Where(s => order(s).CompareTo(req.PageStart) <= 0)
            : source.Where(s => order(s).CompareTo(req.PageStart) >= 0);

        var queried = offsetCollection
            .Take(req.PageSize + 1)
            .ToArray();

        Nullable<TKey> nextPageStart = (queried.Length > req.PageSize)
            ? null
            : new Nullable<TKey>(order(queried[req.PageSize]));

        return factory(
            queried[..req.PageSize].Select(mapping).ToArray(),
            new KeysetPageMeta<TKey> {
                Total = total,
                PageStart = req.PageStart,
                NextPageStart = nextPageStart,
                PageSize = req.PageSize,
            }
        );
    }

    public static TOutPage Paginate<TOutPage, TOut, TSource, TOrder>(
            this IQueryable<TSource> source,
            PagedRequest req,
            Func<TSource, TOut> mapping,
            Func<TOut[], PageMeta, TOutPage> factory,
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
